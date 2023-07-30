using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesCounter : Singleton<LivesCounter>
{
    [Header("Lives")]
    public int health;
    [SerializeField] private int maxHealth = 3;
    public void MinusHealth()
    {
        health -= 1;

        if(health == 0)
        {
            FindObjectOfType<SceneLoader>().GameOver();
        }
    }
}
