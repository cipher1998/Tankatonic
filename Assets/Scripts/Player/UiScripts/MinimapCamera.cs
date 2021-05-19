using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private GameObject Player;

    private void Start()
    {
        Player = FindObjectOfType<Myplayer>().gameObject;
    }
    void LateUpdate()
    {
        if(Player)
        {
            Vector3 pos = Player.transform.position;
            transform.position = new Vector3(pos.x ,pos.y ,-10f);
        }
    }
}
