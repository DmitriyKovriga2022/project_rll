using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public Exit Entrance;
    public event Action ToComeIn;


    public void TakePlayer(GameObject player)
    {
        ToComeIn?.Invoke();
        player.transform.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Entrance.TakePlayer(other.gameObject);
        }
    }
}
