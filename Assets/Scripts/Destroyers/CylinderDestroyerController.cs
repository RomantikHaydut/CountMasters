using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderDestroyerController : Destroyer
{
    void Update()
    {
        Rotate(-1, rotateSpeed);
    }
}
