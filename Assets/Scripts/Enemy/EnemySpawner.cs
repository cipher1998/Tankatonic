using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] Spawnpoints;
    [SerializeField] GameObject Enemy;
    [SerializeField] float Spawnrate;
    private GameObject Player;
    void Start()
    {
        Player = FindObjectOfType<Myplayer>().gameObject;
        InvokeRepeating(nameof(SpawnEnemy),0 , Spawnrate);
    }

   private void SpawnEnemy()
   {
       int random = Random.Range(0 , Spawnpoints.Length);
       GameObject enemy = Instantiate(Enemy , Spawnpoints[random]);
       enemy.transform.parent = null;
   }

   private void Update()
   {
       if(!Player)
       {
           gameObject.SetActive(false);
       }
   }
}
