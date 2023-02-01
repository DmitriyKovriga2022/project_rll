using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : WeaponBase
{
    private Transform _muzzleTransform;
    private Vector2 MuzzlePosition;

    public RangeWeapon(GameObject weapon, string imagePath, int damage, float rateOfFire, Vector2 muzzlePosition) : base(weapon, imagePath, damage, rateOfFire)
    {
        MuzzlePosition = muzzlePosition;

        _muzzleTransform = Weapon.transform.GetChild(1);
        _muzzleTransform.localPosition = MuzzlePosition;
    }
    
    private void Awake() 
    {
    }


    public override void Fire()
    {
        base.Fire();
    }
}
