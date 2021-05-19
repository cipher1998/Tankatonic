using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    
    [SerializeField] Transform[] Spawnpoints;
    private GameObject[] Spawnerobj;
    [SerializeField] GameObject[] Powerup;
    [SerializeField] float Spawnrate;
    [SerializeField] float Destroyrate;
    
    private void Start()
    {
        SetArray();
        InvokeRepeating(nameof(SpawnPowerup), 0f, Spawnrate);
    }

    private IEnumerator Destroyme( GameObject obj)
    {
        yield return new WaitForSeconds(Destroyrate);
        Destroy(obj);

    }

    private void SetArray()
    {
        Spawnerobj = new GameObject[Spawnpoints.Length];
       
    }

    private void SpawnPowerup()
    {
        int index, ind; 
        bool status;
        status = Checkifplacable(out index);
        if(status)
        {
            ind = Random.Range(0, Powerup.Length);
            GameObject obj = Instantiate(
            Powerup[ind],
            Spawnpoints[index].position,
            Spawnpoints[index].rotation
            );
            Spawnerobj[index] = obj;
            obj.transform.parent = null;
            StartCoroutine(Destroyme(obj));
        }
        
        
    }

    private bool Checkifplacable(out int index)
    {
        bool status = false;
        int count = 0;
        index =0;
        while(!status && count <= Spawnerobj.Length)
        {
            count++;
            index = Random.Range(0, Spawnerobj.Length);
            if(Spawnerobj[index] == null)
            {
                status = true;
                break;
                
            }
        }
        return status;
    }
}
