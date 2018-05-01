using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunfishGenerator : MonoBehaviour {

	public Texture2D spriteSheet;
	public LayerMask playerLayer;
	public float weight = 10f;

	private Sprite[] sprites;
	private GameObject[] fishPieces;

	private float fishLength;
	private int numOfDivisions;
	private float spacing;

	// Use this for initialization
	void Awake () {
		sprites = Resources.LoadAll<Sprite> (spriteSheet.name);
		numOfDivisions = sprites.Length;
		fishLength = spriteSheet.width / sprites [0].pixelsPerUnit;
		fishPieces = new GameObject[numOfDivisions];
		spacing = fishLength / numOfDivisions;

		fishPieces [0] = gameObject;
		gameObject.AddComponent<BoxCollider2D> ();
		gameObject.AddComponent<Rigidbody2D> ();

		float pieceWeight = weight / numOfDivisions;

		for (int i = 0; i < sprites.Length; i++) {
			//fishPieces [i].layer = 8;
			SpriteRenderer sr;
			if (i == 0) {
				sr = GetComponent<SpriteRenderer> ();
			} else {
				fishPieces [i] = new GameObject ("Fish[" + i.ToString () + "]");
				sr = fishPieces [i].AddComponent<SpriteRenderer> ();
			}
			fishPieces[i].layer = LayerMask.NameToLayer("Player");

			sr.sprite = sprites [i];

//			int slicePixelHeight = 0;
//			//print (spriteSheet.height);
//			int x = (spriteSheet.width / numOfDivisions * i) + spriteSheet.width / numOfDivisions / 2;
//			Color[] pixels = spriteSheet.GetPixels (x, 0, 1, spriteSheet.height);
//
//			foreach (Color pixel in pixels) {
//				if (pixel.a != 0) {
//					slicePixelHeight++;
//				}
//			}
			//print ("Pixel Height: " + slicePixelHeight + "\tHeight: " + spriteSheet.height +"\tRatio: " + (float)slicePixelHeight / spriteSheet.height);

			BoxCollider2D col;
			if (fishPieces [i].GetComponent<BoxCollider2D> ()) {
				col = fishPieces [i].GetComponent<BoxCollider2D> ();
			} else {
				col = fishPieces [i].AddComponent<BoxCollider2D> ();
			}
			//col.size = new Vector2 (col.size.x * 1.5f, col.size.y * slicePixelHeight / spriteSheet.height * 1.5f);

			Rigidbody2D rb;
			if (fishPieces [i].GetComponent<Rigidbody2D> ()) {
				rb = fishPieces [i].GetComponent<Rigidbody2D> ();
			} else {
				rb = fishPieces [i].AddComponent<Rigidbody2D> ();
			}
			rb.mass = pieceWeight;

			if (i > 0) {
				HingeJoint2D joint = fishPieces [i].AddComponent<HingeJoint2D> ();
				joint.connectedBody = fishPieces [i - 1].GetComponent<Rigidbody2D> ();
				joint.anchor = new Vector2 (-spacing / 2, 0);
				joint.useLimits = true;
				JointAngleLimits2D limits = joint.limits;
				limits.min = 0f;
				limits.max = 0.1f;
				joint.limits = limits;

				fishPieces [i].transform.position = transform.position + Vector3.right * spacing * i;
				fishPieces [i].transform.SetParent (transform);
				//fishPieces [i].transform.localScale = new Vector3 (1.5f, 1f, 1f);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
