using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleWeapon : WeaponBase
{
    private BoxCollider2D _collider2D;
    private Vector2 _colliderOffset, _colliderSize;

    public MeleWeapon(GameObject weapon, string imagePath, int damage, float rateOfFire, Vector2 colliderOffset, Vector2 colliderSize) : base(weapon, imagePath, damage, rateOfFire)
    {
        WeaponType = 0;
        
        _colliderOffset = colliderOffset;
        _colliderSize = colliderSize;

        _collider2D = weapon.GetComponentInChildren<BoxCollider2D>();
    }

    
    public override void ChangeThisWeapon()
    {
        base.ChangeThisWeapon();
        _collider2D.offset = _colliderOffset;
        _collider2D.size = _colliderSize;
    }

    public override void Fire()
    {
        base.Fire();
    }
}
