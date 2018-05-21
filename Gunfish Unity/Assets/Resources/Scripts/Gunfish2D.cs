using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Gunfish2D : NetworkBehaviour {

	public LayerMask groundLayer;
	public float angleFromHorizontal = 30f;
	public float force = 20f;
	public float gunForce = 5000f;
	private float hor, ver;

	private Transform tail;
	[SerializeField] private Transform shootPoint;
	[SerializeField] private bool grounded;
	private bool groundedLF;
	[SerializeField] private AudioClip gunSound;
	[SerializeField] private AudioClip[] flopSounds;

	private AudioSource gunAudioSource;
	private AudioSource flopAudioSource;

	private bool jumpCD;
	private float currentCD = 0f;
	private float maxCD = 0.5f;

	private bool shootCD;
	private float currentShootCD = 0f;
	private float maxShootCD = 0.7f;

	private Rigidbody2D rb;

	private float groundCheckDistance;


	// Use this for initialization
	void Start () {
		gunAudioSource = gameObject.AddComponent<AudioSource> ();
		gunAudioSource.clip = gunSound;
		gunAudioSource.volume = 0.25f;

		flopAudioSource = gameObject.AddComponent<AudioSource> ();
		flopAudioSource.clip = flopSounds[Random.Range(0, flopSounds.Length)];
		flopAudioSource.volume = 0.75f;

		tail = transform.GetChild (transform.childCount-1);
		currentCD = 0f;
		grounded = false;
		groundCheckDistance = GetComponent<BoxCollider2D>().bounds.size.y * 1.2f;
		rb = GetComponent<Rigidbody2D> ();

		hor = Mathf.Cos (angleFromHorizontal * Mathf.Deg2Rad);
		ver = Mathf.Sin (angleFromHorizontal * Mathf.Deg2Rad);

//		int massing = 1;
//		foreach (Transform child in transform) {
//			child.GetComponent<Rigidbody2D> ().mass *= massing++;
//		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		if (Physics2D.Raycast (transform.position, Vector2.down, transform.localScale.y, groundLayer)) {
//			grounded = true;
//		} else {
//			grounded = false;
//		}
		grounded = false;
		foreach (Transform child in transform) {
			if (Physics2D.Raycast (child.position, Vector2.down, groundCheckDistance, groundLayer)) {	
				grounded = true;
			}
		}

		if (grounded && !groundedLF) {
			flopAudioSource.clip = flopSounds[Random.Range(0, flopSounds.Length)];
			flopAudioSource.volume = Mathf.Min (-rb.velocity.y / 100, 1f);
			flopAudioSource.Play ();
		}

		groundedLF = grounded;
	}

	void Update () {
		if (!hasAuthority) {
			return;
		}

		float input = Input.GetAxisRaw ("Horizontal");
		 
		if (jumpCD) {
			currentCD += Time.deltaTime;

			if (currentCD >= maxCD) {
				currentCD = 0f;
				jumpCD = false;
			}
		}
//		print (currentShootCD);
		if (shootCD) {
			currentShootCD += Time.deltaTime;

			if (currentShootCD >= maxShootCD) {
				currentShootCD = 0f;
				shootCD = false;
			}
		}

		if (grounded && !jumpCD && input != 0f) {
			tail.GetComponent<Rigidbody2D>().AddForceAtPosition (new Vector2 (hor * input, ver) * force, tail.position);
//			print ("LMAO");
			jumpCD = true;
		}

		if (!shootCD && Input.GetKeyDown (KeyCode.Space)) {
			Shoot ();
			shootCD = true;
		}
	}

	void Shoot () {
		gunAudioSource.Play ();
		rb.AddForceAtPosition ((shootPoint.parent.position - shootPoint.position).normalized * gunForce, shootPoint.position);
	}
}
