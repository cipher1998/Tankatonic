using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Bullet : MonoBehaviour
{
    public PlayerClaas playerclass;
    private void Start()
    {
        Invoke(nameof(Destroyme) , 3f);
    }

    private void Destroyme()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
             Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "OBSTACLE")
        {
             Destroy(collision.gameObject);
        }
        else
        {
            if(playerclass == PlayerClaas.Enemy)
            {
                collision.gameObject.GetComponent<Health>().TakeDamage(EnemyAI.Mydamage);
            }
            else if(playerclass == PlayerClaas.Player)
            {
                collision.gameObject.GetComponent<Health>().TakeDamage(Myplayer.Mydamage);
            }
            
        }
        Destroy(gameObject);
       
    }
}
