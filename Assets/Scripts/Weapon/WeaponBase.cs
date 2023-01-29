using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase
{
    protected GameObject Weapon;

    public string ImagePath { get; protected set; }
    public int Damage { get; protected set; }
    public float RateOfFire { get; protected set; }


    public WeaponBase(GameObject weapon, string imagePath, int damage, float rateOfFire)
    {
        Weapon = weapon;
        ImagePath = imagePath;
        Damage = damage;
        RateOfFire = rateOfFire;
    }
    
    public virtual void Fire()
    {
        Debug.Log("Fire");
    }
}
