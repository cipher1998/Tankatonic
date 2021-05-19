using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveTurret : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{

    private RectTransform rectTransform;
    private Vector2 startPos;
    private Vector2 ParentMaxMin;
    private Vector2 FirstTouch;
    
   [SerializeField] GameObject Turret;
   [SerializeField] Transform SpawnPoint;
   [SerializeField] GameObject Bullet;
    
    [SerializeField] float BulletShootoffset;
    [SerializeField] float BulletDelay;

    [SerializeField] float BulletVelocity;
    private IEnumerator co;
    private bool Isshooting =false;
    private bool cooldown = false;
    private void Start()
    {
        co= SpawnBullet();
        rectTransform =  GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        ParentMaxMin = transform.parent.GetComponent<RectTransform>().sizeDelta - rectTransform.sizeDelta;
        BulletShootoffset = Mathf.Abs(ParentMaxMin.x);
    }

    // Update is called once per frame
    void Update()
    {
          OnkeyboardInput();
          OnturretFire();
    }

    private void OnturretFire()
    {
       if(Input.GetMouseButtonDown(0))
       {
           if(!Isshooting && !cooldown)
                {
                    Isshooting =true;
                    StartCoroutine(co);
                }

       }
       else if(Input.GetMouseButtonUp(0))
       {
            Isshooting = false;
                cooldown = true;
                Invoke(nameof(ResetIsShooting) , 2f);
                StopCoroutine(co);
       }
    }

    private void OnkeyboardInput()
    {
        if(!Application.isFocused) { return ;}
        Vector3 pos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3(pos.x , pos.y ,0f);
       
         Vector2 relativepos = pos - Turret.transform.position ;
      
       float angle = Mathf.Atan2(relativepos.x , relativepos.y)*Mathf.Rad2Deg +  Turret.transform.parent.rotation.eulerAngles.z;
       Quaternion rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
       Turret.transform.localRotation = rotation;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
            rectTransform.anchoredPosition = eventData.position-FirstTouch;

            rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x,-ParentMaxMin.x,ParentMaxMin.x)
             ,Mathf.Clamp(rectTransform.anchoredPosition.y,-ParentMaxMin.y,ParentMaxMin.y));
            DoRotation( rectTransform.anchoredPosition );

            if(rectTransform.anchoredPosition.magnitude > BulletShootoffset)
            {
                if(!Isshooting && !cooldown)
                {
                    Isshooting =true;
                    StartCoroutine(co);
                }

            }
            else if(Isshooting)
            {
                Isshooting = false;
                cooldown = true;
                Invoke(nameof(ResetIsShooting) , 2f);
                StopCoroutine(co);
            }
        }
    }

    IEnumerator SpawnBullet()
    {
        while(true)
        {
            GameObject bulletclone = Instantiate(Bullet, SpawnPoint.position ,SpawnPoint.rotation);
            bulletclone.GetComponent<Rigidbody2D>().velocity = bulletclone.transform.up * BulletVelocity;
            bulletclone.GetComponent<Bullet>().playerclass = PlayerClaas.Player;
            yield return new WaitForSeconds(BulletDelay);
        }
      
    }

    private void DoRotation(Vector2 position)
    {
         Vector2 relativepos = position - startPos;
       float angle = Mathf.Atan2(relativepos.x , relativepos.y)*Mathf.Rad2Deg;
       Quaternion rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
       Turret.transform.localRotation = rotation;
    }



    public void OnEndDrag(PointerEventData eventData)
    {
         rectTransform.anchoredPosition = startPos;
        if(!cooldown && Isshooting)
        {
            cooldown = true;
            Invoke(nameof(ResetIsShooting) , 2f);
            StopCoroutine(co);
        }

         Turret.transform.localRotation = new Quaternion(0f,0f,0f,1f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
         FirstTouch = eventData.position;
    }

    private void ResetIsShooting()
    {
        cooldown = false;
    }
}
