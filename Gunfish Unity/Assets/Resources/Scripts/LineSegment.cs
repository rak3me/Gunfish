using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour {

    public LineRenderer segment;
    public int index;
	
	// Update is called once per frame
	void Update () {
        segment.SetPosition(index, transform.position);
	}
}
