using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MoveExplosive : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{

    private RectTransform rectTransform;
    private Vector2 startPos;
    private Vector2 ParentMaxMin;
    private Vector2 FirstTouch;
    
   [SerializeField] GameObject Turret;
     [SerializeField] Transform SpawnPoint;
     [SerializeField] GameObject Explosive;
   [SerializeField] float Cooldown;
    [SerializeField] float ExplosiveShootoffset;

    [SerializeField] float ExplosiveVelocity;
   private float useTime;
   private bool Canuse;



   private Image MyImage;
  
    private void Start()
    {

        MyImage = GetComponent<Image>();
        Canuse = true;
        rectTransform =  GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        ParentMaxMin = transform.parent.GetComponent<RectTransform>().sizeDelta - rectTransform.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
          if(Time.time - useTime >= Cooldown)
          {
              Canuse = true;
              useTime = Time.time;
               MyImage.color = new Color(1f,1f,1f, 0.6f);
          }

        //  OnkeyboardInput();
          OnturretFire();
    }

    private void OnturretFire()
    {
       if(Input.GetMouseButtonDown(1))
       {
          if(Canuse)
          {
              SpawnExplosive();
          }
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
          
             if(rectTransform.anchoredPosition.magnitude > ExplosiveShootoffset)
            {
               if(Canuse)
               {
                   SpawnExplosive();
               }
            }

        }
    }

    private void SpawnExplosive()
    {
        
            GameObject Explosiveclone = Instantiate(Explosive, SpawnPoint.position ,SpawnPoint.rotation);
            Explosiveclone.GetComponent<Rigidbody2D>().velocity = Explosiveclone.transform.up * ExplosiveVelocity;
            Canuse = false;
            useTime = Time.time;
            MyImage.color = new Color(1f,1f,1f, 0.3f);
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
         Turret.transform.localRotation = new Quaternion(0f,0f,0f,1f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
         FirstTouch = eventData.position;
    }

}
