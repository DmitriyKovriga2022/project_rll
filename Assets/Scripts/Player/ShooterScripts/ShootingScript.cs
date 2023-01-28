using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingScript : MonoBehaviour
{
    private Rigidbody2D ourWeapon;
    private Vector2 _mousePosition;
    public Camera cam;
    public GameObject bullet;
    private float bulletForce = 20f;
    public Transform firePoint;

    private void Awake()
    {
        ourWeapon = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = _mousePosition - ourWeapon.position; //если вычесть вектор из вектора, то мы получим прямую линию от одного к другому :)
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        ourWeapon.rotation = angle;
    }

    public void setMousePos(InputAction.CallbackContext context)
    {
        Debug.Log("MouseSetPos");
        _mousePosition = cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    public void shoot()
    {
        GameObject currentBullet = Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
        currentBullet.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * bulletForce, ForceMode2D.Impulse);
    }
}
