using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour, ITarget
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject camo;
    [SerializeField] private float hiddenRatio = 0.1f;
    [SerializeField] private float stealthRatio = 0.5f;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject projectile;

    private static readonly int Attack = Animator.StringToHash("Attack");

    private Animator _animator;
    private TreasureChest _treasureChest;
    private Camera _mainCamera;

    private bool _isMoving;
    private bool _isHidden;
    private bool _isStealth;
    private bool _isAiming;
    private bool _isTouchingTreasure;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _treasureChest = GameObject.FindGameObjectWithTag("Treasure").GetComponent<TreasureChest>();
        _mainCamera = Camera.main;
    }

    public float VisualDetectionDistance(float nominalDistance)
    {
        var visualDetectionRatio = 1.0f;
        if (_isHidden) visualDetectionRatio = hiddenRatio;
        else if (_isStealth) visualDetectionRatio = stealthRatio;

        return nominalDistance * visualDetectionRatio;
    }

    private void Hide(bool hidden)
    {
        _isHidden = hidden;
        body.SetActive(!hidden);
        camo.SetActive(hidden);
    }

    private void Fire()
    {
        var newProjectile = Instantiate(projectile);
        newProjectile.transform.position = throwPoint.position;
        newProjectile.transform.rotation = Random.rotation;
        var direction = _mainCamera.transform.forward;
        newProjectile.GetComponent<Rigidbody>().AddForce(direction * 4f, ForceMode.Impulse);
    }

    private void OnMove(InputValue value)
    {
        _isMoving = value.Get<Vector2>() != Vector2.zero;
        if (_isHidden)
            Hide(false);
    }

    private void OnHide(InputValue value)
    {
        if (!value.isPressed || _isMoving)
            return;

        Hide(!_isHidden);
    }

    private void OnOpen(InputValue value)
    {
        if (_isTouchingTreasure)
            _treasureChest.Open();
    }

    private void OnStealth(InputValue value)
    {
        _isStealth = value.isPressed;
    }

    private void OnFire(InputValue value)
    {
        if (_isAiming) _animator.SetTrigger(Attack);
    }

    private void OnAim(InputValue value)
    {
        _isAiming = value.isPressed;
    }

    private void AttackEnd()
    {
        Fire();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure"))
            _isTouchingTreasure = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Treasure"))
            _isTouchingTreasure = false;
    }
}
