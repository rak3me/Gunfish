using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]
public class LineSegment : MonoBehaviour {

    public LineRenderer segment;
    public int index;
    public bool grounded;

    private RaycastHit2D hit1;
    private RaycastHit2D hit2;

    Ray ray1;
    Ray ray2;

    private Vector2 bounds;
    private Vector2 offset;
    float distance;

    private void Start () {
        bounds = GetComponent<BoxCollider2D>().bounds.extents;
        offset = GetComponent<BoxCollider2D>().offset;
        distance = 2f * 1.5f * Mathf.Sqrt(bounds.x * bounds.x + bounds.y * bounds.y);
    }

    // Update is called once per frame
    void Update () {
        segment.SetPosition(index, transform.position);

        ray1 = new Ray(transform.position + (transform.right.normalized * -bounds.x + transform.up.normalized * bounds.y) * 1.5f, (transform.right.normalized * bounds.x + transform.up.normalized * -bounds.y) * 1.5f);
        ray2 = new Ray(transform.position + (transform.right.normalized * bounds.x + transform.up.normalized * bounds.y) * 1.5f, (transform.right.normalized * -bounds.x + transform.up.normalized * -bounds.y) * 1.5f);
        hit1 = Physics2D.Raycast(ray1.origin, ray1.direction, distance, LayerMask.GetMask("Ground"));
        hit2 = Physics2D.Raycast(ray2.origin, ray2.direction, distance, LayerMask.GetMask("Ground"));
        if (hit1 || hit2) {
            grounded = true;
            print("Segment " + index + " grounded..." + "LayerMask: ");
        } else {
            grounded = false;
        }

        Debug.DrawRay(ray1.origin, ray1.direction, Color.red);
        Debug.DrawRay(ray2.origin, ray2.direction, Color.green);
	}

    //private void OnCollisionStay2D (Collision2D collision) {
    //    if (collision.collider.tag == "Ground") {
    //        grounded = true;
    //    }
    //}

    //private void OnCollisionExit (Collision collision) {
    //    if (collision.collider.tag == "Ground") {
    //        grounded = false;
    //    }
    //}
}
