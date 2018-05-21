using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LineSegment : MonoBehaviour {

    public LineRenderer segment;
    public int index;

	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
        segment.SetPosition(index, transform.position);
	}
}
