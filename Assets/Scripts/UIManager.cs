using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public void DisplayText(Text textObject,string text)
    {
        textObject.text = text;
    }
}
