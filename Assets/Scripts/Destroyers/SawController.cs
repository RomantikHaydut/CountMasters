using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : Destroyer
{
    public override void Awake()
    {
        base.Awake();
    }
    void Update()
    {
        Movement(GoingLeft(), speed);
    }

    public override void Movement(bool goingLeft, float speed)
    {
        base.Movement(goingLeft, speed);
        int rotateDir = goingLeft ? -1 : 1;
        Rotate(rotateDir, rotateSpeed);
    }

    private bool GoingLeft()
    {
        if (transform.position.x < -boundryX)
            goingLeft = false;
        if (transform.position.x > boundryX)
            goingLeft = true;

        return goingLeft;
    }
}
