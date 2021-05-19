using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Myplayer : MonoBehaviour
{
    
    [SerializeField] float Myscore;
    [SerializeField] GameObject text;
    [SerializeField] GameObject GameoverScreen;
     public static float Mydamage;
     public static float Myspeed;
    public void IncreaseMyscore(float value)
    {
        Myscore += value;
        text.GetComponent<TextMeshProUGUI>().text = $"Score: {Myscore}";
    }

    private void Start()
    {
        
        Mydamage = 50f;
        Myspeed = 1f;
         GetComponent<Health>().OnzeroHealth += HandleDeath;
    }

    
    private void OnDestroy()
    {
         GetComponent<Health>().OnzeroHealth -= HandleDeath;
    }
    private void HandleDeath()
    {
        GameoverScreen.SetActive(true);
        Destroy(gameObject);
        Time.timeScale = 0;

    }

}
