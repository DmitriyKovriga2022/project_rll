using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float _speed = 15;
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
    }

    private void OnEnable() {
        Invoke("Death", 10);
    }

    void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_transform.position + _transform.right.normalized *_speed * Time.fixedDeltaTime);
    }

    private void Death()
    {
        gameObject.SetActive(false);
    }
}
