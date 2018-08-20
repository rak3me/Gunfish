using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public float force = 3000f;

    [SerializeField] private float currentShootCD = 0f;
    public float maxShootCD = 0.7f;

    public AudioClip fireSound;
    public AudioSource gunAudioSource;

    private void Start () {
        if (!fireSound) {
            fireSound = Resources.Load<AudioClip>("Audio/RifleShot");
        }

        gunAudioSource = gameObject.AddComponent<AudioSource>();
        gunAudioSource.clip = fireSound;
    }

    // Update is called once per frame
    void Update () {
        if (currentShootCD >= maxShootCD) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Fire();
            }
        } else {
            currentShootCD += Time.deltaTime;
        }
    }

    void Fire () {
        gunAudioSource.Play ();

        transform.parent.GetComponent<Rigidbody2D>().AddForceAtPosition((Vector2)transform.right * -force, (Vector2)transform.position);
        currentShootCD = 0f;
    }
}
