using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    [SerializeField] private float _dashForce;
    private float _speed;
    private Vector2 _moveDirection, _dashDirection;
    private bool _isOnDash;

    private Interactor _interactor;

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _interactor = GetComponent<Interactor>();

        _speed = GetComponent<PlayerCharacteristics>().Speed;
    }

    
    void FixedUpdate()
    {
        if(_isOnDash)
        {
            Dash();
        }
        else
        {
            Move();
        }
        Look();
    }

    private void Dash()
    {
        _rigidbody2D.AddForce(_dashDirection*_dashForce);
    }

    private void Move()
    {
        if (_moveDirection != Vector2.zero)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + _moveDirection.normalized * _speed * Time.fixedDeltaTime);
        }
    }

    private void Look()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = Camera.main.nearClipPlane;
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        LookDirection = (worldMousePosition - (Vector2)transform.position).normalized;
        _animator.SetFloat("LookDirectionX", LookDirection.x);
        _animator.SetFloat("LookDirectionY", LookDirection.y);
    }


    public void OnMove(InputValue input)
    {
        _moveDirection = input.Get<Vector2>();
    }

    public void OnDash()
    {
        if (!_isOnDash && _moveDirection != Vector2.zero) StartCoroutine(DashSwitch());
    }

    public void OnInteract()
    {
        _interactor.Interact();
    }

    private IEnumerator DashSwitch()
    {
        _dashDirection = _moveDirection.normalized;
        _isOnDash = true;

        yield return new WaitForSeconds(0.5f);

        _isOnDash = false;
        _dashDirection = Vector2.zero;
    }
}
