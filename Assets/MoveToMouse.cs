using UnityEngine;
using System.Collections;

public class MoveToMouse : MonoBehaviour {
	public Vector3 elbowPosition;
	private Vector3 focalPoint;
	private bool isHitThisFrame;
	private Vector3 clickFocalPoint;
	private Vector3 formerPosition;


	void Start () {
	}
	
	void Update () {
		// move the geometry of the ball... all of this to be replaced with a Vive controller
		float horiz = SuperLerpUnclamped (-5.0f, 5.0f, 0.0f, Screen.width, Input.mousePosition.x);
		float vert = SuperLerpUnclamped (0.0f, 10.0f, 0.0f, Screen.height, Input.mousePosition.y);
		transform.position = new Vector3 (horiz, vert, 3.0f);

		int layerMask = 1 << 8;
		RaycastHit[] hits = Physics.RaycastAll(elbowPosition, transform.position - elbowPosition, Mathf.Infinity, layerMask);

		isHitThisFrame = (hits.Length > 0);
		if (isHitThisFrame) {
			RaycastHit ground = hits[0];
			focalPoint = ground.point;

			if (Input.GetMouseButtonDown(0)) {
				clickFocalPoint = focalPoint;
				formerPosition = ground.transform.position;
			}
			if (Input.GetMouseButton (0)) {
				ground.transform.position = focalPoint - clickFocalPoint + formerPosition;
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, elbowPosition);
		if (isHitThisFrame) {
			Gizmos.DrawLine(transform.position, focalPoint);
		}
	}

	// uhm, i'm sure there's a way to componentize this, but I'll leave that til I get someone who can teach me how to do this.  Source:
	// http://wiki.unity3d.com/index.php?title=SpeedLerp&action=edit
	// althought, to be fair, this can get removed as soon as this is hooked up to a Vive anyhow
	private static float SuperLerpUnclamped (float from, float to, float from2, float to2, float value) {
		return (to - from) * ((value - from2) / (to2 - from2)) + from;
	}
}
