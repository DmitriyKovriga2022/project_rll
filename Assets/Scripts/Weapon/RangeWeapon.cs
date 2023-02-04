using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : WeaponBase
{
    private PoolExemplar _bulletPool;
    private Transform _muzzleTransform;
    private Vector2 _muzzlePosition;

    public RangeWeapon(GameObject weapon, string imagePath, int damage, float rateOfFire, Vector2 muzzlePosition) : base(weapon, imagePath, damage, rateOfFire)
    {
        WeaponType = 1;
        
        _muzzlePosition = muzzlePosition;

        _muzzleTransform = Weapon.transform.GetChild(1);
        
        _bulletPool = GameObject.Find("Bullets").GetComponent<PoolExemplar>();
    }

    public override void ChangeThisWeapon()
    {
        base.ChangeThisWeapon();
        _muzzleTransform.localPosition = _muzzlePosition;
    }

    public override void Fire()
    {
        _bulletPool.CreateObject(_muzzleTransform.position, _muzzleTransform.rotation);
        base.Fire();
    }
}
