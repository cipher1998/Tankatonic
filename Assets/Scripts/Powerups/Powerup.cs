using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] float mydamageIncrease;
    [SerializeField] float myspeedincrease;
    [SerializeField] float myhealthincrease;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Myplayer.Mydamage *=mydamageIncrease;
            Myplayer.Myspeed  *=myspeedincrease;
            col.gameObject.GetComponent<Health>().Myhealth *= myhealthincrease;
            Destroy(gameObject);
        }
    }
    
}
