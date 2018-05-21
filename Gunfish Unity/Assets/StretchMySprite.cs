using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//changes sprite's x scale to adjust to changes in the fish's segment positions

//hopes to fix the skeletoning issue.

public class StretchMySprite : MonoBehaviour {
	//the previous segment on the fish if it exists
	public Transform backSegment;
	//the next segment on the fish if it exists
	public Transform forwardSegment;
	//my sprite
	public SpriteRenderer sr;

	//current frame
	private float distBack; //distance between centers of this segment and previous
	private float distForward; //distance between centers of this segment and next
	private float angleBack; //angle between this segment and previous
	private float angleForward; //angle between this segment and next

	//previous frame
	private float prevDistBack;
	private float prevDistForward;
	private float prevAngleBack;
	private float prevAngleForward;


	void Start(){
		sr = GetComponent<SpriteRenderer> ();
		prevDistBack = (backSegment.position - transform.position).magnitude;
		prevDistForward = (forwardSegment.position - transform.position).magnitude;
		prevAngleBack = 0;
		prevAngleForward = 0;
	}

	// Update is called once per frame
	void Update () {
		distBack = (backSegment.position - transform.position).magnitude;
		distForward = (forwardSegment.position - transform.position).magnitude;
		angleBack = backSegment.eulerAngles.z - transform.eulerAngles.z;
		angleForward = forwardSegment.eulerAngles.z - transform.eulerAngles.z;

		float newX = transform.localScale.x;
		newX *= distBack / prevDistBack; //account for the percentage change in distance
		newX *= distForward / prevDistForward; //account for the percentage change in distance

		transform.localScale = new Vector3 (newX,transform.localScale.y, transform.localScale.z);

		prevDistBack = distBack;
		prevDistForward = distForward;
		prevAngleBack = angleBack;
		prevAngleForward = angleForward;
	}
}
