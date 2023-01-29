using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacteristics : MonoBehaviour
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int HealthRegeneration { get; private set; }
    public int MeleeDamage { get; private set; }
    public int MeleeDamageModifier { get; private set; }
    public int RangedDamageModifier { get; private set; }
    public int MagicDamageModifier { get; private set; }
    public int CriticalHitChance { get; private set; }
    public int CriticalDamageMultiplier { get; private set; }
    public int Speed { get; private set; }
    public int Luck { get; private set; }
    public int SciencePointsPerTurn { get; private set; }
    public int NumberOfSciencePoints { get; private set; }
    public int UsefulMaterialsPerTurn { get; private set; }
    public int NumberOfUsefulMaterials { get; private set; }

    private PlayerCharacteristicsData _data;
    private SaveLoadJson<PlayerCharacteristicsData> _saveLoadJson = new();

    void Awake()
    {
        LoadCharacteristics();
    }

    public void LoadCharacteristics()
    {
        _data = _saveLoadJson.LoadData(SaveLoadJson<PlayerCharacteristicsData>.PathName.StartCharacteristics);

        MaxHealth = _data.MaxHealth;
        CurrentHealth = _data.CurrentHealth;
        HealthRegeneration = _data.HealthRegeneration;
        MeleeDamage = _data.MeleeDamage;
        MeleeDamageModifier = _data.MeleeDamageModifier;
        RangedDamageModifier = _data.RangedDamageModifier;
        MagicDamageModifier = _data.MagicDamageModifier;
        CriticalHitChance = _data.CriticalHitChance;
        CriticalDamageMultiplier = _data.CriticalDamageMultiplier;
        Speed = _data.Speed;
        Luck = _data.Luck;
        SciencePointsPerTurn = _data.SciencePointsPerTurn;
        NumberOfSciencePoints = _data.NumberOfSciencePoints;
        UsefulMaterialsPerTurn = _data.UsefulMaterialsPerTurn;
        NumberOfUsefulMaterials = _data.NumberOfUsefulMaterials;
    }

    public void SaveCharacteristics()
    {
        _data.MaxHealth = MaxHealth;
        _data.CurrentHealth = CurrentHealth;
        _data.HealthRegeneration = HealthRegeneration;
        _data.MeleeDamage = MeleeDamage;
        _data.MeleeDamageModifier = MeleeDamageModifier;
        _data.RangedDamageModifier = RangedDamageModifier;
        _data.MagicDamageModifier = MagicDamageModifier;
        _data.CriticalHitChance = CriticalHitChance;
        _data.CriticalDamageMultiplier = CriticalDamageMultiplier;
        _data.Speed = Speed;
        _data.Luck = Luck;
        _data.SciencePointsPerTurn = SciencePointsPerTurn;
        _data.NumberOfSciencePoints = NumberOfSciencePoints;
        _data.UsefulMaterialsPerTurn = UsefulMaterialsPerTurn;
        _data.NumberOfUsefulMaterials = NumberOfUsefulMaterials;

        _saveLoadJson.SaveData(_data, SaveLoadJson<PlayerCharacteristicsData>.PathName.SavedCharacteristics);
    }
}
