using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class WordChecker : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private string answer; 
    public void AnswerChecker()
    {
        if (inputField.text == answer)
        {
            Debug.Log("correct");
            inputField.text = null; 
        }
        else
        {
            Debug.Log("Wrong");
            inputField.text = null; 
        }
    }
}
