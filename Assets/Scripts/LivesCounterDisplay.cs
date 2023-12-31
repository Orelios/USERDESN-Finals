using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class LivesCounterDisplay : MonoBehaviour
{
    TextMeshProUGUI text;
    public LivesCounter livesCounter;
    public GameObject player;
    public static LivesCounterDisplay instance; 
    void Awake()
    {
        instance = this; 
        livesCounter = LivesCounter.Instance;
    }
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "LIVES: " + livesCounter.health;
    }

   
    void Update()
    {
        text.text = "LIVES: " + livesCounter.health; 
    }
}
