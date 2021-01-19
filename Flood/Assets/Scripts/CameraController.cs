using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	const float SMOOTHING = 0.3f;

	private Vector3 velocity = Vector3.zero;
	private float angVelocity = 0;
	private float targetAng = 0;


	private float zoomVelocity = 0;
	private float targetZoom = 100;

	const float minZoom = 20;
	const float maxZoom = 200;

	float zoom = maxZoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 dir = Vector3.zero;
		if (Input.GetKey(KeyCode.W))
			dir.z += 1;
		if (Input.GetKey(KeyCode.A))
			dir.x -= 1;
		if (Input.GetKey(KeyCode.S))
			dir.z -= 1;
		if (Input.GetKey(KeyCode.D))
			dir.x += 1;
		if (Input.GetKeyDown(KeyCode.Q))
			targetAng += 60;
		if (Input.GetKeyDown(KeyCode.E))
			targetAng -= 60;

		
  
  
		


		targetZoom = Mathf.Clamp(targetZoom - 10*Input.mouseScrollDelta.y, minZoom, maxZoom);
		zoom = Mathf.SmoothDamp(zoom, targetZoom, ref zoomVelocity, 0.2f);

		Vector3 newPos = transform.position + Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * Vector3.SmoothDamp(Vector3.zero, 10 * dir, ref velocity, SMOOTHING);
		newPos.y = zoom;
		
		transform.position = newPos;



		transform.rotation = Quaternion.Euler(Mathf.Lerp(55, 85, Mathf.InverseLerp(minZoom, maxZoom, zoom)), Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAng, ref angVelocity, 0.15f), 0);

		//transform.LookAt(new Vector3(0, 30, 0), Vector3.up);

		//Mathf.
	}
}
