using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float smoothing = 5f;

	private Vector3 offset;

	void Start()
	{
		if(target != null) offset = transform.position - target.position;
	}

	void FixedUpdate()
	{
		if(target==null)
        {
			var player = FindObjectOfType<PlayerHealth>();
			if (player == null) return;

			target = player.transform;
			offset = transform.position - target.position;
		}

		Vector3 targetCamPos = target.position + offset;
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
