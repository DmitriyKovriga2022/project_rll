using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingScript : MonoBehaviour
{
    
}


/*private Vector3 target;
public Camera cam;

public GameObject bullet;

private float bulletForce = 25f;


private void Update()
{
    Vector3 difference = target - transform.rotation.eulerAngles;
    float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
}

public void setMousePos(InputAction.CallbackContext context)
{
    target = cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
}

public void shoot(InputAction.CallbackContext context)
{
    if (context.performed)
    {
        GameObject projectile = Instantiate(bullet, gameObject.transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletForce, ForceMode2D.Impulse);
    }
}*/
