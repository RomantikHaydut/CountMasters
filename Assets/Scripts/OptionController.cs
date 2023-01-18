using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{
    public bool isOptionPositive;
    public bool isOptionMultiplier;

    public int myValue;

    private CloneController cloneController;

    private void Awake()
    {
        cloneController = FindObjectOfType<CloneController>();
    }


    public void MyFunction()
    {
        int stickCount = cloneController.stickCount;
        if (isOptionPositive)
        {
            if (isOptionMultiplier)
            {
                cloneController.CloneStickman4(stickCount * (myValue - 1));
            }
            else
            {
                cloneController.CloneStickman4(myValue);
            }
        }
        else
        {
            if (isOptionMultiplier)
            {
                cloneController.DestroyStickmans3((int)(stickCount * ((float)myValue / 100)));
            }
            else
            {
                cloneController.DestroyStickmans3(myValue);
            }
        }

        transform.parent.gameObject.SetActive(false);

    }
}
