using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private InputActionReference changeVeapon;
    private Weapon _weapon;

    private DataList<MeleWeaponData> _meleWeaponsDataList = new();
    private DataList<RangeWeaponData> _rangeWeaponsDataList = new();

    private SaveLoadJson<DataList<MeleWeaponData>> _saveLoadMeleWeaponData = new();
    private SaveLoadJson<DataList<RangeWeaponData>> _saveLoadRangeWeaponData = new();

    private List<WeaponBase> _weaponsInInventory = new();
    private int _currentWeapon = 0;

    void Awake()
    {
        _weapon = GetComponent<Weapon>();
        changeVeapon.action.performed += OnChangeWeapon;
        LoadData();
    }

    private void Start() {
        _weapon.SetWeapon(_weaponsInInventory[_currentWeapon]);
    }

    
    private void LoadData()
    {
        _meleWeaponsDataList = _saveLoadMeleWeaponData.LoadData(JsonPathNames.MeleWeaponsCharacteristicsPath);
        _rangeWeaponsDataList = _saveLoadRangeWeaponData.LoadData(JsonPathNames.RangeWeaponsCharacteristicsPath);

        foreach (var meleWeaponData in _meleWeaponsDataList.objectsList)
        {
            Vector2 colliderOffset = new Vector2(meleWeaponData.ColliderOffset[0], meleWeaponData.ColliderOffset[1]);
            Vector2 colliderSize =  new Vector2(meleWeaponData.ColliderSize[0], meleWeaponData.ColliderSize[1]);
            _weaponsInInventory.Add(new MeleWeapon(gameObject, meleWeaponData.ImagePath, meleWeaponData.Damage, meleWeaponData.RateOfFire,colliderOffset, colliderSize));
        }
        foreach (var rangeWeaponData in _rangeWeaponsDataList.objectsList)
        {
            Vector2 muzzlePosition = new Vector2(rangeWeaponData.MuzzlePosition[0], rangeWeaponData.MuzzlePosition[1]);
            _weaponsInInventory.Add(new RangeWeapon(gameObject, rangeWeaponData.ImagePath, rangeWeaponData.Damage, rangeWeaponData.RateOfFire, muzzlePosition));
        }
    }


    public void OnChangeWeapon(InputAction.CallbackContext context)
    {
        _currentWeapon++;
        if (_currentWeapon == _weaponsInInventory.Count)
        {
            _currentWeapon = 0;
        }
        _weapon.SetWeapon(_weaponsInInventory[_currentWeapon]);
    }    
}
