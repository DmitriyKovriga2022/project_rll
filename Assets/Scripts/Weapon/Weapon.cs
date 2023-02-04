using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] private InputActionReference fire;
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private WeaponBase _currentWeapon;
    private MovementController _movementController;

    public Vector2 _lookDirection;


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _transform = transform;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _movementController = GetComponentInParent<MovementController>();

        fire.action.started += StartedFire;
        fire.action.canceled += CanceledFire;
    }

    public void SetWeapon(WeaponBase weapon)
    {
        _currentWeapon = weapon;
        
        _animator.SetTrigger("ChangeWeapon");
    }

    private void LateUpdate() {
        Aim();
    }

    private void Aim()
    {
        _lookDirection = _movementController.LookDirection;
        Quaternion rotation = Quaternion.FromToRotation(_transform.right, _lookDirection);
        _transform.rotation = rotation * _transform.rotation;
    }

    private void StartedFire(InputAction.CallbackContext context)
    {
        _animator.SetBool("IsFire", true);
    }

    private void CanceledFire(InputAction.CallbackContext context)
    {
        _animator.SetBool("IsFire", false);
    }

    public void Fire()
    {
        _currentWeapon.Fire();
    }

    public void ChangeWeaponCharacteristics()
    {
        _currentWeapon.ChangeThisWeapon();
        _animator.SetFloat("RateOfFire", _currentWeapon.RateOfFire);
        _animator.SetInteger("WeaponType", _currentWeapon.WeaponType);
        _spriteRenderer.sprite = _currentWeapon.Image;
    }
}
