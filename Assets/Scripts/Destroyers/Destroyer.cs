using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destroyer : MonoBehaviour
{
    public float speed = 1f;
    public float rotateSpeed = 1f;

    private CloneController cloneController;
    public float boundryX;
    public bool goingLeft;

    public virtual void Awake()
    {
        cloneController = FindObjectOfType<CloneController>();
        boundryX = FindObjectOfType<PlayerController>().boundryX;
    }
    public virtual void Movement(bool goingLeft_ , float speed)
    {
        if (goingLeft_)
        {
            transform.position -= Vector3.right * Time.deltaTime * speed;
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
    }

    public virtual void Rotate(int direction , float rotateSpeed)
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed * 360 * direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Stickman")
        {
            cloneController.DestroySpecificStickman(other.gameObject.transform.parent.gameObject);
        }
    }
}
