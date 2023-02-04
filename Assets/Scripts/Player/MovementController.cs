using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    [SerializeField] private float _dashForce;
    private float _speed;
    private Vector2 _moveDirection, _dashDirection;


    
    private enum State {Idle, Run, Dash};
    private State _currentState;
    public Vector2 LookDirection { get; private set; }


    private Interactor _interactor;

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _interactor = GetComponent<Interactor>();

        _speed = GetComponent<PlayerCharacteristics>().Speed;
    }

    
    void FixedUpdate()
    {
        if(_currentState == State.Idle)
        {
            Idle();
        }
        if(_currentState == State.Run)
        {
            Run();
        }
        else if(_currentState == State.Dash)
        {
            Dash();
        }
        Look();
    }

    private void Dash()
    {
        _rigidbody2D.AddForce(_dashDirection*_dashForce);
    }

    private void Idle()
    {
        if(_moveDirection != Vector2.zero)
        {
            _currentState = State.Run;
            SetAnimationState();
        }
    }

    private void Run()
    {
        if (_moveDirection == Vector2.zero)
        {
            _currentState = State.Idle;
            SetAnimationState();
            return;
        }
        _rigidbody2D.MovePosition(_rigidbody2D.position + _moveDirection.normalized * _speed * Time.fixedDeltaTime);
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

    private void SetAnimationState()
    {
        _animator.SetBool("IsRun", _currentState == State.Run);
        _animator.SetBool("IsDash", _currentState == State.Dash);
    }


    public void OnMove(InputValue input)
    {
        _moveDirection = input.Get<Vector2>();
    }

    public void OnDash()
    {
        if (_currentState == State.Run) StartCoroutine(DashSwitch());
    }

    public void OnInteract()
    {
        _interactor.Interact();
    }

    private IEnumerator DashSwitch()
    {
        _dashDirection = _moveDirection.normalized;
        _currentState = State.Dash;
        SetAnimationState();

        yield return new WaitForSeconds(0.5f);

        _currentState = State.Idle;
        SetAnimationState();
        _dashDirection = Vector2.zero;
    }
}
