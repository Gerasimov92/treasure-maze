using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ITarget
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject camo;

    private GameController _gameController;
    private bool _isMoving;
    private bool _isHidden;
    private float _visualDetectionRatio = 1;

    void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure"))
            _gameController.Win();
    }
}
