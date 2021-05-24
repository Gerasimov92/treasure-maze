using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ITarget
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject camo;

    private Vector2 _moveVector;
    private bool _isHidden;
    private float _visualDetectionRatio = 1;

    void Update()
    {
        Move(_moveVector);
    }

    private void OnMove(InputValue value)
    {
        if (_isHidden)
            Hide(false);

        _moveVector = value.Get<Vector2>();
    }

    private void OnHide(InputValue value)
    {
        if (!value.isPressed)
            return;

        Hide(!_isHidden);
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;

        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        transform.position += move * scaledMoveSpeed;
    }

    private void Hide(bool hidden)
    {
        _isHidden = hidden;
        body.SetActive(!hidden);
        camo.SetActive(hidden);
        _visualDetectionRatio = hidden ? 0.1f : 1.0f;
    }

    public float VisualDetectionDistance(float nominalDistance)
    {
        return nominalDistance * _visualDetectionRatio;
    }
}
