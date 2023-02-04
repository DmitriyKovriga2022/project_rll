using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsCRIPT : MonoBehaviour
{
    public int damage = 100;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Разбился об " + collision.gameObject.ToString());
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9)
            Debug.Log("Разбился об " + collision.gameObject.ToString());
        Destroy(gameObject);
    }
}
