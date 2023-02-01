using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleWeapon : WeaponBase
{
    public MeleWeapon(GameObject weapon, string imagePath, int damage, float rateOfFire) : base(weapon, imagePath, damage, rateOfFire){}

    public override void Fire()
    {
        base.Fire();
    }
}
