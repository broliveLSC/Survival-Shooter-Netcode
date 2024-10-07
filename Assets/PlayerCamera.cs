using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Transform target;
    private Vector3 offset;
    public float smoothing = 5f;
    Vector3 origin;

    float orthoSizeDefault;

    // Start is called before the first frame update
    void Awake()
    {
        origin = transform.position;
        orthoSizeDefault = GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null) offset = transform.position - target.position;

        GetComponent<Camera>().orthographicSize = 8;
    }

    public void NoTarget()
    {
        transform.position = origin;
        GetComponent<Camera>().orthographicSize = orthoSizeDefault;
    }

}
