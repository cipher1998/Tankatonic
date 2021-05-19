using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveDash : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] GameObject Tank;

    private Rigidbody2D rigidbody;
    [SerializeField] float Cooldown;
    [SerializeField] float DashStrength;

    [SerializeField] float DashTime;
    [SerializeField] GameObject DashEffect;
    private float usetime;
    private bool Canuse;


    private Image MyImage;  

    void Start()
    {
        rigidbody= Tank.GetComponent<Rigidbody2D>();
        Canuse = true;
        MyImage = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - usetime >= Cooldown)
        {
            usetime = Time.time ; Canuse = true;
            MyImage.color = new Color(1f,1f,1f, 0.6f);
          
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(Canuse)
            {
                DoDash();
            }
        }
    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if(Canuse)
        {
            DoDash();
        }
       
    }

    private void DoDash()
    {
        rigidbody.velocity = Tank.transform.up * DashStrength;
        DashEffect.SetActive(true);
        StartCoroutine(StopDash());
        Canuse= false;
        MyImage.color = new Color(1f,1f,1f, 0.3f);
    }
    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(DashTime);
        rigidbody.velocity = Vector2.zero;
        DashEffect.SetActive(false);
    }
}
