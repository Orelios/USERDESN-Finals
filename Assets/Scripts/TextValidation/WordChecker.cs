using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class WordChecker : MonoBehaviour
{
    [Header("Screen")]
    public GameObject textValidationScreen;
    public GameObject input; 

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

    [Header("Display answer")]
    [SerializeField] private TextMeshProUGUI displayAnswerText; 
    [SerializeField] private string[] displayAnswer;

    private int questionNumber = 1;
    private void Start()
    {
        displayAnswerText.CrossFadeAlpha(0.0f, 0, true);
    }
    public void Update()
    {
        DisplayQuestion();
    }
    public void InputAsnwer()
    {
        bool ans;
        try
        {
            displayAnswer[questionNumber] = inputField.text;
            DisplayAnswers();
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
        catch
        {
            input.SetActive(false); 
        }
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
            displayAnswerText.CrossFadeAlpha(1.0f, 0, true);

        }
    }

    public bool CheckAnswer(bool ans)
    {
        return ans; 
    }

    public void DisplayAnswers()
    {
        if(displayAnswer != null)
        {
            displayAnswerText.text = displayAnswer[questionNumber - 1] + "Your answer for " + questionNumber + " is " 
                + displayAnswer[questionNumber] + "\n";
            displayAnswer[questionNumber] = displayAnswerText.text; 
        }
       
    }

    public void CloseTab()
    {
        textValidationScreen.SetActive(false); 
    }
}
