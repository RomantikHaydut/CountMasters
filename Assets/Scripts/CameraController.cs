using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;

    private Vector3 offset;

    [SerializeField] private float followSpeed = 1f;
    private void Awake()
    {
        target = FindObjectOfType<PlayerController>().transform;
        if (target == null)
            Debug.LogError("There is no target for camera , please put a PlayerController object to scene.");
        offset = target.position - transform.position;
    }

    
    void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 smoothPos = Vector3.Lerp(transform.position, target.position - offset, Time.deltaTime * followSpeed);
        transform.position = smoothPos;
    }
}
