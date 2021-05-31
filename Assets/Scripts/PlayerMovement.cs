using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private float stealthSpeed = 1;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float runSpeed = 7;

    private static readonly int Speed = Animator.StringToHash("Speed");

    private CharacterController _controller;
    private Animator _animator;
    private Camera _mainCamera;

    private float _moveSpeed;
    private Vector2 _moveVector;
    private bool _isRunning;
    private bool _isStealth;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        Move(_moveVector);
    }

    private void Move(Vector2 input)
    {
        if (input.sqrMagnitude < 0.01)
        {
            Idle();
            return;
        }

        if (_isStealth) Stealth();
        else Run();

        var targetDirection = CalcTargetDirection(input).normalized;

        UpdateRotation(targetDirection);

        targetDirection *= _moveSpeed;
        _controller.Move(targetDirection * Time.deltaTime);
    }

    private void Idle()
    {
        _animator.SetFloat(Speed, 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        _moveSpeed = walkSpeed;
        _animator.SetFloat(Speed, 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        _moveSpeed = runSpeed;
        _animator.SetFloat(Speed, 1, 0.1f, Time.deltaTime);
    }

    private void Stealth()
    {
        _moveSpeed = stealthSpeed;
        _animator.SetFloat(Speed, -1, 0.1f, Time.deltaTime);
    }

    private Vector3 CalcTargetDirection(Vector2 input)
    {
        var forward = _mainCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        var right = _mainCamera.transform.TransformDirection(Vector3.right);
        return input.x * right + input.y * forward;
    }

    private void UpdateRotation(Vector3 direction)
    {
        var freeRotation = Quaternion.LookRotation(direction, transform.up);
        var differenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
        var eulerY = transform.eulerAngles.y;

        if (differenceRotation < 0 || differenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
        var euler = new Vector3(0, eulerY, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        _moveVector = value.Get<Vector2>();
    }

    private void OnRun(InputValue value)
    {
        _isRunning = value.isPressed;
    }

    private void OnStealth(InputValue value)
    {
        _isStealth = value.isPressed;
    }
}
