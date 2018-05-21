using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunfishSpriteHandler : MonoBehaviour {

	Sprite sprite;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ().sprite;
		Vector2[] vertices = new Vector2[]{ new Vector2(-1f, 1f), new Vector2(1f, 1f), new Vector2(-0.5f, -1f), new Vector2(0.5f, -1f) };
		sprite.OverrideGeometry (vertices, sprite.triangles);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
