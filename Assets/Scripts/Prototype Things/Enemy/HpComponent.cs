using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpComponent : MonoBehaviour
{
    [SerializeField]private int hp; 
    [SerializeField]private int maxHp;

    public LayerMask bulletLayer;
    

    private void GetDamage(int value)
    {
        hp = hp - value;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void GetHeal(int value)
    {
        hp = hp + value;
        if (hp>maxHp)
        {
            hp = maxHp;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GetDamage(collision.gameObject.GetComponent<BulletsCRIPT>().damage);
        }
    }
}
