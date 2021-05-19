using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    
    private Transform Target;
    [Header("PlayerChase")]
    [SerializeField] float Speed;
    [SerializeField] float NextWaypointDistance ;
    [SerializeField] float Range;
    [SerializeField] float ChaseRange;

    [Header("BulletShoot")]
    [SerializeField] GameObject Myturret;
    [SerializeField] Transform BulletSpawnPoint;
    [SerializeField ] public static float Mydamage;
    [SerializeField ]  float Mypoints;
    [SerializeField] GameObject Bullet;
    [SerializeField] float BulletDelay;
     [SerializeField] float BulletVelocity;
    
    private Path path;
    private int CurrentWaypoint =0;
    private bool ReachedEndpoint =false;

    private Seeker seeker;
    private Rigidbody2D Rb;
    private IEnumerator co;
    private bool coroutinstarted;
    void Start()
    {
        Mydamage = 10f;
        GetComponent<Health>().OnzeroHealth += HandleDeath;
        GameObject g = GameObject.FindGameObjectWithTag("Player");  if(!g) { return ;}
        Target = g.transform;
        seeker = GetComponent<Seeker>();
        Rb = GetComponent<Rigidbody2D>();
        co= BulletSpawn();
        coroutinstarted = false;
        InvokeRepeating(nameof(Updatepath), 0f,0.3f);
        
    }

     void OnDestroy()
    {
        GetComponent<Health>().OnzeroHealth -= HandleDeath;
    }
    private void HandleDeath()
    {
        if(!Target) { return ;}
        Target.GetComponent<Myplayer>().IncreaseMyscore(Mypoints);
        Destroy(gameObject);
    }

    private float DistanceBetweenTarget()
    {
        return Vector2.Distance(transform.position , Target.position);
    }
    private void Updatepath()
    {
        if(Target == null) { return ;}
        if(DistanceBetweenTarget() > ChaseRange && seeker.IsDone())
        {
            seeker.StartPath(transform.position , Target.position , OnPathCalculated);
        }
    }
    private void OnPathCalculated(Path p)
    {
        if(!p.error)
        {
            path = p;
            CurrentWaypoint = 0;
        }
    }

    void Update()
    {
        if(!Target) { return; }
        CheckDistance();
       
    }

    private void CheckDistance()
    {
        if(DistanceBetweenTarget() <= ChaseRange) 
        {
            path =null;
            StartShooting();
            return;
        }
        else
        {
          
            StopShooting();
        }

        if(path == null) { return ;}
        if(CurrentWaypoint > path.vectorPath.Count)
        {
            ReachedEndpoint = true;
            return;
        }
        else
        {
            ReachedEndpoint = false;
        }

       
        float distance = Vector2.Distance((Vector2)transform.position , path.vectorPath[CurrentWaypoint]);
        RotateTowardsTarget(path.vectorPath[CurrentWaypoint] , transform);
        Vector2 force = transform.up*Speed * Time.deltaTime;
        Rb.AddForce(force);

        if(distance <= NextWaypointDistance)
        {
            CurrentWaypoint++;
        }
    }

    private void RotateTowardsTarget(Vector3 target, Transform mypostion)
    {
        Vector2 direction = (Vector2)(target -  mypostion.position);
        float angle = Mathf.Atan2(direction.x , direction.y)*Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
        mypostion.rotation = Quaternion.Lerp(mypostion.rotation , rotation , 1f * Time.deltaTime);
    }

    private void StartShooting()
    {
        
        RotateTowardsTarget(Target.position , Myturret.transform);
         
        if(!coroutinstarted)// && Myturret.transform.rotation == Target.rotation)
        {
            StartCoroutine(co);
            coroutinstarted = true;
        }
    }

    private void StopShooting()
    {
        StopCoroutine(co);
        coroutinstarted = false;
    }
    private IEnumerator BulletSpawn()
    {
        while(true)
        {
            GameObject bulletclone = Instantiate(Bullet, BulletSpawnPoint.position ,BulletSpawnPoint.rotation);
            bulletclone.GetComponent<Rigidbody2D>().velocity = bulletclone.transform.up * BulletVelocity;
            bulletclone.GetComponent<Bullet>().playerclass = PlayerClaas.Enemy;
            yield return new WaitForSeconds(BulletDelay);
        }
    }

   
}
