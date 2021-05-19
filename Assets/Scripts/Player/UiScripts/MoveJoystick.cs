using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveJoystick : MonoBehaviour, IDragHandler , IEndDragHandler, IBeginDragHandler
{
    private RectTransform rectTransform;
    private Vector2 startPos;
    private Vector2 ParentMaxMin;
    private Vector2 FirstTouch;
    
   [SerializeField] GameObject Tank;

   [SerializeField] float speedclamp;
   private Vector2 Speed;
    private void Start()
    {
        Speed= new Vector2(0f,0f);
        rectTransform =  GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        ParentMaxMin = transform.parent.GetComponent<RectTransform>().sizeDelta - rectTransform.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
         MoveForward(Speed); 
        if(Debug.isDebugBuild)
        {
            OnKeyboardInput();    
        }
    }

    private void OnKeyboardInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
           
            Tank.transform.position += Tank.transform.up * y * Myplayer.Myspeed * Time.deltaTime * 3f;
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
       {
           
             Tank.transform.Rotate(Vector3.forward* 50 * -x * Time.deltaTime);
        }
    
 
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
            rectTransform.anchoredPosition = eventData.position-FirstTouch;

            rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x,-ParentMaxMin.x,ParentMaxMin.x)
             ,Mathf.Clamp(rectTransform.anchoredPosition.y,-ParentMaxMin.y,ParentMaxMin.y));
            DoRotation( rectTransform.anchoredPosition );
            Speed = (rectTransform.anchoredPosition - startPos)/speedclamp;
        }
    }

    private void DoRotation(Vector2 position)
    {
         Vector2 relativepos = position - startPos;
       float angle = Mathf.Atan2(relativepos.x , relativepos.y)*Mathf.Rad2Deg;
       Quaternion rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
       Tank.transform.localRotation = rotation;
    }

    private void MoveForward(Vector2 speed)
    {
       
        Tank.transform.position += new Vector3(speed.x , speed.y , 0f)* Myplayer.Myspeed * Time.deltaTime;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
         rectTransform.anchoredPosition = startPos;
         Speed= new Vector2(0f,0f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
         FirstTouch = eventData.position;
    }
}
