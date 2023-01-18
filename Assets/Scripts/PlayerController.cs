using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Animator anim;
    // Movement.
    [SerializeField] private bool mouseMovement;
    [SerializeField] private float forwardSpeed = 1f;
    [SerializeField] private float horizontalSpeed = 1f;
    private float mouseClickPosX;
    private float mouseActivePosX;
    public float boundryX = 5f; // Ground boundry


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        //Forward movement.
        transform.position += Vector3.forward * Time.deltaTime * forwardSpeed;

        //Horizontal Movement keyboard.
        if (!mouseMovement)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.position += Vector3.right * Time.deltaTime * horizontalInput * horizontalSpeed;
        }
        //Horizontal Movement mouse.
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseClickPosX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                mouseActivePosX = Input.mousePosition.x;
                float horizontalInput = (mouseActivePosX - mouseClickPosX <= 0) ? -1 : 1;
                transform.position += Vector3.right * Time.deltaTime * horizontalInput * horizontalSpeed;
            }
        }
    }

    // Animations
    private void StartRun()
    {
        if (!anim.GetBool("Running"))
        {
            anim.SetBool("Running", true);
        }
    }


}
