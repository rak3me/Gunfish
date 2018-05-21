using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(LineRenderer))]
public class GunfishGenerator : NetworkBehaviour {

	public Texture2D spriteSheet;
	public LayerMask playerLayer;
	public float weight = 10f;

	private Sprite[] sprites;
	private GameObject[] fishPieces;

	private float fishLength, fishHeight;
	private int numOfDivisions;
	private float spacing;

	private LineRenderer lineFish;

	// Use this for initialization
	void Awake () {
		sprites = Resources.LoadAll<Sprite> (spriteSheet.name);
		numOfDivisions = sprites.Length;
		fishLength = spriteSheet.width / sprites [0].pixelsPerUnit;
		fishHeight = spriteSheet.height / sprites [0].pixelsPerUnit;
		fishPieces = new GameObject[numOfDivisions];
		spacing = fishLength / numOfDivisions;
		GetComponent<SpriteRenderer> ().sprite = sprites [0];

		fishPieces [0] = gameObject;
		gameObject.AddComponent<BoxCollider2D> ();
		gameObject.AddComponent<Rigidbody2D> ();

		float pieceWeight = weight / numOfDivisions;

		lineFish = GetComponent<LineRenderer> ();

        lineFish.positionCount = sprites.Length;
        lineFish.startWidth = fishHeight;
		lineFish.endWidth = fishHeight;
		lineFish.alignment = LineAlignment.Local;

		for (int i = 0; i < sprites.Length; i++) {
			//fishPieces [i].layer = 8;
			SpriteRenderer sr;
			if (i == 0) {
				sr = GetComponent<SpriteRenderer> ();
			} else {
				fishPieces [i] = new GameObject ("Fish[" + i.ToString () + "]");
				sr = fishPieces [i].AddComponent<SpriteRenderer> ();
			}

            LineSegment segment = fishPieces[i].AddComponent<LineSegment>();
            segment.segment = lineFish;
            segment.index = i;

            fishPieces[i].layer = LayerMask.NameToLayer("Player");

			sr.sprite = sprites [i];

			//int slicePixelHeight = 0;
			//print (spriteSheet.height);
			int x = (spriteSheet.width / numOfDivisions * i) + spriteSheet.width / numOfDivisions / 2;
			Color[] pixels = spriteSheet.GetPixels (x, 0, 1, spriteSheet.height);

			int sliceStartPixel = 0;
			int sliceEndPixel = pixels.Length - 1;
			//int sliceMidPoint = pixels.Length / 2;

//			foreach (Color pixel in pixels) {
//				if (pixel.a != 0) {
//					slicePixelHeight++;
//				}
//			}

			for (int j = 0; j < pixels.Length; j++) {
				if (pixels [j].a != 0) {
					sliceStartPixel = j;
					break;
				}
			}

			for (int j = pixels.Length - 1; j >= 0; j--) {
				if (pixels [j].a != 0) {
					sliceEndPixel = j;
					break;
				}
			}

			//print ("Pixel Height: " + slicePixelHeight + "\tHeight: " + spriteSheet.height +"\tRatio: " + (float)slicePixelHeight / spriteSheet.height);

			BoxCollider2D col;
			if (fishPieces [i].GetComponent<BoxCollider2D> ()) {
				col = fishPieces [i].GetComponent<BoxCollider2D> ();
			} else {
				col = fishPieces [i].AddComponent<BoxCollider2D> ();
			}
			float ySize = col.size.y * (sliceEndPixel - sliceStartPixel) / spriteSheet.height * 1.1f;
			ySize = Mathf.Clamp (ySize, 0.6f, spriteSheet.height / sprites [0].pixelsPerUnit);
			col.size = new Vector2 (col.size.x * 1.25f, ySize);
			//col.offset = new Vector2 (0f, 1/((sliceEndPixel - sliceStartPixel) / sprites [0].pixelsPerUnit));

			Rigidbody2D rb;
			if (fishPieces [i].GetComponent<Rigidbody2D> ()) {
				rb = fishPieces [i].GetComponent<Rigidbody2D> ();
			} else {
				rb = fishPieces [i].AddComponent<Rigidbody2D> ();
			}
			rb.mass = pieceWeight;
			rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

			if (i > 0) {
				HingeJoint2D joint = fishPieces [i].AddComponent<HingeJoint2D> ();
				joint.connectedBody = fishPieces [i - 1].GetComponent<Rigidbody2D> ();
				joint.anchor = new Vector2 (-spacing / 2, 0);
				joint.useLimits = true;
				JointAngleLimits2D limits = joint.limits;
				limits.min = 0f;
				limits.max = 1f;
				joint.limits = limits;

				fishPieces [i].transform.position = transform.position + Vector3.right * spacing * i;
				fishPieces [i].transform.SetParent (transform);
				//fishPieces [i].transform.localScale = new Vector3 (1.5f, 1f, 1f);
			}
			Destroy (sr);


			//Network Handling
			/******************************************************/
			if (i == 0) {
				continue;
			}

			//print (gameObject.name);
			gameObject.SetActive(false);
			NetworkTransformChild ntc = this.gameObject.AddComponent<NetworkTransformChild>();
			ntc.enabled = true;
			ntc.sendInterval = 20;
			ntc.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisZ;
			ntc.target = fishPieces [i].transform;
			gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
