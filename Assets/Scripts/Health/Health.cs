using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    
    [SerializeField] public float Myhealth;
    [SerializeField] float Displaytime;
    private float Maximumhealth;
    [SerializeField] Image HealthBar;
     [SerializeField] GameObject Healtcanvas;
    public delegate void HandlezeroHealth();
    public  event HandlezeroHealth OnzeroHealth;
    
    private void Start()
    {
        Maximumhealth = Myhealth;
    }
    public void TakeDamage(float Damage)
    {
        if(Myhealth >= 0 )
        {
            Myhealth -= Damage;
            //temporary
            if(Myhealth> Maximumhealth ) { Maximumhealth = Myhealth; }
            HealthBar.fillAmount  = Myhealth / Maximumhealth;
            Healtcanvas.SetActive(true);
            Invoke(nameof(Deactivehealthbar) , Displaytime);
        }
        if(Myhealth <= 0)
        {
            OnzeroHealth?.Invoke();
            return ;
        }
       
    }

    private void Deactivehealthbar()
    {
       Healtcanvas.SetActive(false);
    }
}
