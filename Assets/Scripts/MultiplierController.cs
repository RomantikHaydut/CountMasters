using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierController : MonoBehaviour
{
    public GameObject option1;
    public GameObject option2;

    public Text option1Text;
    public Text option2Text;

    private CloneController cloneController;
    private UIManager uiManager;

    private void Awake()
    {
        cloneController = FindObjectOfType<CloneController>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void SetRandomOptions()
    {
        float randomValue = Random.value;
        if (randomValue <= 0.33f) // One bad one good option
        {
            SetOptionsMixed();
        }
        else if (randomValue > 0.33f && randomValue <= 0.66f) // Both good
        {
            SetBothOptionsGood();
        }
        else if (randomValue > 0.66f && randomValue <= 1f) // Both bad
        {
            SetBothOptionsBad();
        }
    }

    private void SetRandomGoodOption(GameObject option, Text text)
    {
        OptionController optionController = option.GetComponent<OptionController>();
        float randomValue = Random.value;
        if (randomValue >= 0.5f) // Plus
        {
            SetPlusOption(optionController, text);

        }
        else // Multiplier
        {
            SetMultiplierOption(optionController, text);
        }

        optionController.isOptionPositive = true;

    }


    private void SetRandomBadOption(GameObject option, Text text)
    {
        OptionController optionController = option.GetComponent<OptionController>();
        float randomValue = Random.value;
        if (randomValue >= 0.5f) // minus
        {
            SetMinusOption(optionController, text);
        }
        else // divide
        {
            SetDivideOption(optionController, text);
        }

        optionController.isOptionPositive = false;
    }

    #region Positive Options
    private void SetPlusOption(OptionController optionController, Text text)
    {
        int plusFactor = Random.Range(1, cloneController.maxCloneCount / 10);
        string text_ = "+ " + plusFactor.ToString();
        uiManager.DisplayText(text, text_);
        text.color = Color.green;
        text.transform.parent.GetComponent<Image>().color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.1f);
        optionController.isOptionMultiplier = false;
        optionController.myValue = plusFactor;
    }

    private void SetMultiplierOption(OptionController optionController, Text text)
    {
        int multiplierFactor = Random.Range(2, 11);
        string text_ = "X " + multiplierFactor.ToString();
        uiManager.DisplayText(text, text_);
        text.color = Color.green;
        text.transform.parent.GetComponent<Image>().color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.1f);
        optionController.isOptionMultiplier = true;
        optionController.myValue = multiplierFactor;
    }
    #endregion
    #region Negative Options
    private void SetMinusOption(OptionController optionController, Text text)
    {
        int minusValue = Random.Range(1, cloneController.maxCloneCount / 10);
        string text_ = "- " + minusValue.ToString();
        uiManager.DisplayText(text, text_);
        text.color = Color.red;
        text.transform.parent.GetComponent<Image>().color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.1f);
        optionController.isOptionMultiplier = false;
        optionController.myValue = minusValue;
    }
    private void SetDivideOption(OptionController optionController , Text text)
    {
        int divideValue = Random.Range(10, 100);
        string text_ = "- %" + divideValue.ToString();
        uiManager.DisplayText(text, text_);
        text.color = Color.red;
        text.transform.parent.GetComponent<Image>().color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.1f);
        optionController.isOptionMultiplier = true;
        optionController.myValue = divideValue;
    }
    #endregion
    public void SetBothOptionsGood()
    {
        SetRandomGoodOption(option1, option1Text);
        SetRandomGoodOption(option2, option2Text);
    }

    public void SetBothOptionsBad()
    {
        SetRandomBadOption(option1, option1Text);
        SetRandomBadOption(option2, option2Text);
    }

    public void SetOptionsMixed()
    {
        float randomOption = Random.value;
        if (randomOption >= 0.5f) // Right
        {
            SetRandomGoodOption(option1, option1Text);
            SetRandomBadOption(option2, option2Text);
        }
        else // Left
        {
            SetRandomGoodOption(option2, option2Text);
            SetRandomBadOption(option1, option1Text);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.transform.position . x <= 0)
            {
                option1.GetComponent<OptionController>().MyFunction();
            }
            else
            {
                option2.GetComponent<OptionController>().MyFunction();
            }
            this.enabled = false;
        }

    }

}
