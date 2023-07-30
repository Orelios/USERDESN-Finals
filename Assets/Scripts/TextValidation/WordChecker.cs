using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class WordChecker : MonoBehaviour
{
    [Header("Question")]
    [SerializeField] private string[] question;
    [SerializeField] private TextMeshProUGUI displayQuestions;

    [Header("Choices")]
    [SerializeField] private TextMeshProUGUI Choice1;
    [SerializeField] private TextMeshProUGUI Choice2;
    [SerializeField] private TextMeshProUGUI Choice3;
    [SerializeField] private string[] choice1;
    [SerializeField] private string[] choice2;
    [SerializeField] private string[] choice3;

    [Header("Answer")]
    [SerializeField] private string []answer;
    [SerializeField] private TMP_InputField inputField;

    


    private int questionNumber = 1; 

    public void Update()
    {
        DisplayQuestion();
    }
    public void InputAsnwer()
    {
        bool ans; 
       
        if (inputField.text == answer[questionNumber])
        {
            Debug.Log("correct");
            inputField.text = null;
            ans = true;
            questionNumber++;
        }
        else
        {
            Debug.Log("Wrong");
            inputField.text = null;
            ans = false;
            questionNumber++;
        }

        CheckAnswer(ans);
        ActionsManager.instance.DecrementActionCounter();
    }

    public void DisplayQuestion()
    {
        try
        {
            displayQuestions.text = question[questionNumber];
            Choice1.text = choice1[questionNumber];
            Choice2.text = choice2[questionNumber];
            Choice3.text = choice3[questionNumber];
        }
        catch
        {
            displayQuestions.text = question[0];
            Choice1.text = choice1[0];
            Choice2.text = choice2[0];
            Choice3.text = choice3[0];
        }
    }

    public bool CheckAnswer(bool ans)
    {
        return ans; 
    }
}
