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

    private Vector2 _lookDirection;

    private DataList<RangeWeaponData> _rangeWeaponsDataList = new();
    //private DataList<MeleWeaponData> _meleWeaponsDataList = new();
    public RangeWeaponData _rangeWeaponData = new();
    //public MeleWeaponData _meleWeaponData = new();
    private SaveLoadJson<DataList<RangeWeaponData>> _saveLoadRangeWeaponData = new();
    //private SaveLoadJson<DataList<MeleWeaponData>> _saveLoadMeleWeaponData = new();

    private void Awake()
    {
        Initialize();
        LoadData();
        InitializeWeapon(2);
    }

    private void LoadData()
    {
        _rangeWeaponsDataList = _saveLoadRangeWeaponData.LoadData(SaveLoadJson<DataList<RangeWeaponData>>.PathName.SavedWeapons);
        //_meleWeaponsDataList = _saveLoadMeleWeaponData.LoadData();
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

    private void InitializeWeapon(int id)
    {
        _rangeWeaponData = _rangeWeaponsDataList.objectsList[id];

        Vector2 muzzlePosition = new Vector2(_rangeWeaponData.MuzzlePosition[0], _rangeWeaponData.MuzzlePosition[1]);

        _currentWeapon = new RangeWeapon(gameObject, _rangeWeaponData.ImagePath, _rangeWeaponData.Damage, _rangeWeaponData.RateOfFire, muzzlePosition);

        _spriteRenderer.sprite = Resources.Load<Sprite>(_currentWeapon.ImagePath);
        _animator.SetFloat("RateOfFire", _currentWeapon.RateOfFire);
    }

    private void LateUpdate() {
        Aim();
    }

    private void Aim()
    {
        _lookDirection = _movementController.LookDirection;
        Quaternion rotation = Quaternion.FromToRotation((Vector2)_transform.right + new Vector2(0, _rangeWeaponData.MuzzlePosition[1]) , _lookDirection*10 + (Vector2)_transform.localPosition);
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
}
