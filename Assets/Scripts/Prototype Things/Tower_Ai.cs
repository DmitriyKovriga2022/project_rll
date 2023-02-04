using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Ai : MonoBehaviour
{
    
    public float range;
    public float fireRate = 0.2f;
    public float projectileSpeed;

    [SerializeField] public GameObject bullet;

    private float cooldown = 0f;
    private Vector2 point;

    private void Awake()
    {
        point = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        Collider2D[] enemies = Physics2D.OverlapCircleAll(point, range);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.tag == "Enemy")
            {
                if (cooldown <= 0f)
                {
                    Vector3 dir = enemy.transform.position - transform.position;
                    dir.Normalize();

                    GameObject projectile = Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
                    projectile.GetComponent<Rigidbody2D>().AddForce(dir * projectileSpeed, ForceMode2D.Impulse);
                    cooldown = fireRate;
                }
                else
                {
                    cooldown -= Time.deltaTime;
                }
            }
        }
    }
}
