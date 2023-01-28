using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsCRIPT : MonoBehaviour
{
    public int damage = 100;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
