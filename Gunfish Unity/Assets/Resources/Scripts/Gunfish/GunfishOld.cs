using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GunfishOld : MonoBehaviour {

	Rigidbody rb;
	Vector2 moveDirection;
	public float speedForce = 10f;
	public float timeBetweenForces = 0.25f;
	bool youreGroundedYoungMan;
	public Transform rootyTootyPointandShooty;

	// Use this for initialization
	void Start () {
		youreGroundedYoungMan = false;
		InvokeRepeating ("PushMrsJohnsonPush", timeBetweenForces, timeBetweenForces);
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		moveDirection = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (Input.GetKeyDown (KeyCode.Space)) {
			HitMeWithYourBestShot ();
		}
	}

	void LateUpdate () {
		youreGroundedYoungMan = false;
	}

	void HitMeWithYourBestShot () {
		rb.AddForceAtPosition(-rootyTootyPointandShooty.forward * 1000, rootyTootyPointandShooty.position);
	}

	void PushMrsJohnsonPush () {
		if (moveDirection != Vector2.zero) {
			rb.AddTorque (Random.insideUnitSphere * 100000);
			rb.AddForceAtPosition (new Vector3 (moveDirection.x * speedForce, Random.Range (200, 240), moveDirection.y * speedForce) / rb.mass, -transform.forward * 2);
		}
	}

	void onCollisionStay (Collision other) {
		youreGroundedYoungMan = true;
	}
}
