using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LivesCounter : Singleton<LivesCounter>
{
    [Header("Lives")]
    public int health;
    public int maxHealth = 3;
    public int tutorialHealth = 6; 
    public void MinusHealth()
    {
        health -= 1;

        if(health == 0)
        {
            FindObjectOfType<NotesManager>().ClearSavedNotes();
            FindObjectOfType<SceneLoader>().GameOver();
        }
    }

    public void SetHealthToMaxHealth()
    {
        health = maxHealth;
    }

    public void SetHealthToTutorialHealth()
    {
        health = tutorialHealth;
    }
}
