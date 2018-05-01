using System;
using UnityEngine;


public class Camera2DFollow : MonoBehaviour
{
	public Transform target;
	public float smoothTime = 0.3f;
	Vector3 vel = Vector3.zero;

	void Update() {
		Vector2 averagePoint = Vector2.zero;
		foreach (Transform child in target) {
			averagePoint += (Vector2)child.position;
		}
		averagePoint /= target.childCount;

		Vector3 smoothTarget = new Vector3(averagePoint.x, averagePoint.y, -10);
		transform.position = Vector3.SmoothDamp (transform.position, smoothTarget, ref vel, smoothTime);

	}
}
