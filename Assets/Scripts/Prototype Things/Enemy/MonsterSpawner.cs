using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public bool isActive;
    public int monsterForSpawn = 5;
    public float TimeBetweenSpawn = 5;
    private float timer;

    public GameObject monsterPrefab;

    private void Awake()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TimeBetweenSpawn && isActive && monsterForSpawn > 0)
        {
            SpawnMonster();
        }
    }

    private void SpawnMonster()
    {
        Instantiate(monsterPrefab, gameObject.transform);
        monsterForSpawn--;
        timer = 0;
    }
}
