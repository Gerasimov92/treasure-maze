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

    private bool _isMoving;
    private bool _isHidden;
    private bool _isStealth;
    private bool _isTouchingTreasure;
    private int _attackLayerIndex;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _attackLayerIndex = _animator.GetLayerIndex("Attack Layer");
        _treasureChest = GameObject.FindGameObjectWithTag("Treasure").GetComponent<TreasureChest>();
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
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 2, ForceMode.Impulse);
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
        StartCoroutine(AttackAnimation());
    }

    private void AttackEnd()
    {
        Fire();
    }

    private IEnumerator AttackAnimation()
    {
        _animator.SetLayerWeight(_attackLayerIndex, 1);
        _animator.SetTrigger(Attack);

        yield return new WaitForSeconds(1);

        _animator.SetLayerWeight(_attackLayerIndex, 0);
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
