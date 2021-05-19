using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] float EffRadius;
    [SerializeField] float Damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
       Collider2D []colliders = Physics2D.OverlapCircleAll(gameObject.transform.position ,EffRadius);
       foreach(Collider2D c in colliders)
       {
           if(c.gameObject.TryGetComponent<Health>(out Health health))
           {
               health.TakeDamage(Damage);
           }
           else
           {
               Destroy(c.gameObject);
           }
           
       }
       Destroy(gameObject);
    }

}
