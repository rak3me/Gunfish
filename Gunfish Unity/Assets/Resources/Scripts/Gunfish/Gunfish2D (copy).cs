using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Gunfish2DCopy : MonoBehaviour {

	public LayerMask groundLayer;
	public float angleFromHorizontal = 30f;
	public float force = 20f;
	private float hor, ver;

	[SerializeField] private bool grounded;
	private bool groundedLF;
	public AudioClip[] flopSounds;

    Transform tail;
	private AudioSource flopAudioSource;

	private bool jumpCD;
	private float currentCD = 0f;
	private float maxCD = 0.5f;

	private Rigidbody2D rb;

	private float groundCheckDistance;

    public void ApplyVariableDefaults () {
        groundLayer = 1;
        angleFromHorizontal = 30f;
        force = 200f;

        flopSounds = new AudioClip[] { Resources.Load<AudioClip>("Audio/Flop1"), Resources.Load<AudioClip>("Audio/Flop2"), Resources.Load<AudioClip>("Audio/Flop3") };
    }

    public void SetGrounded (bool ground) {
        grounded = ground;
    }

	// Use this for initialization
	void Start () {
        
        tail = transform.GetChild(transform.childCount - 2);

		flopAudioSource = gameObject.AddComponent<AudioSource> ();
		flopAudioSource.clip = flopSounds[Random.Range(0, flopSounds.Length)];
		flopAudioSource.volume = 0.75f;

		currentCD = 0f;
		grounded = false;

        if (!GetComponent<BoxCollider2D>()) {
            //gameObject.AddComponent<BoxCollider2D>();
        }

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
            if (child.GetSiblingIndex() == transform.childCount - 1) {
                break;
            }
            if (child.GetComponent<LineSegment>().grounded) {	
				grounded = true;
                break;
			}
		}

		if (grounded && !groundedLF) {
            if (flopAudioSource == null) {
                flopAudioSource = gameObject.AddComponent<AudioSource>();
            }
			flopAudioSource.clip = flopSounds[Random.Range(0, flopSounds.Length)];
//			flopAudioSource.volume = Mathf.Min (-rb.velocity.y / 100, 1f);
			flopAudioSource.Play ();
		}

		groundedLF = grounded;
	}

	void Update () {
		//if (!hasAuthority) {
		//	return;
		//}

		float input = Input.GetAxisRaw ("Horizontal");
		 
		if (jumpCD) {
			currentCD += Time.deltaTime;

			if (currentCD >= maxCD) {
				currentCD = 0f;
				jumpCD = false;
			}
		}

        if (!jumpCD && Mathf.Abs(input) > Mathf.Epsilon) {
			//tail.GetComponent<Rigidbody2D>().AddForceAtPosition (new Vector2 (hor * input, ver) * force, tail.position);
            if (grounded) {
                GetComponent<Rigidbody2D>().AddForce(new Vector2 (hor * input, ver) * force);
            }

            GetComponent<Rigidbody2D>().AddTorque(-input * force * Random.Range(0.5f, 1f));
			jumpCD = true;
		}
	}

    public void Move (Vector2 force, Vector2 torque) {

    }
}
