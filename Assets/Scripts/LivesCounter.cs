using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesCounter : MonoBehaviour
{
    public static LivesCounter instance; 
    public int health;
    [SerializeField] private int maxHealth = 3;
    public SceneLoader sceneloader;
    private void Awake()
    {
        instance = this; 
    }

    void Update()
    {
        if(health == 0)
        {
            sceneloader.GameOver();
        }
    }

    public void MinusHealth()
    {
        health -= 1;
    }
}
