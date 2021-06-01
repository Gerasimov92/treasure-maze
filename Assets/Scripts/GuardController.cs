using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    [SerializeField] private bool loopPath;
    [SerializeField] private List<Transform> points;

    [SerializeField] private Transform tempPoint;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private Animator _animator;
    private NavMeshAgent _agent;
    private TargetDetector _targetDetector;
    private GameController _gameController;

    private int _currentPointIndex = 0;
    private int _dir = 1;
    private bool _checkInProgress;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _targetDetector = GetComponent<TargetDetector>();
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (points.Count >= 2)
            SetDestination(_currentPointIndex);
    }

    void Update()
    {
        if (_gameController.GameEnded)
        {
            _agent.SetDestination(transform.position);
            _animator.SetFloat(Speed, 0);
        }
        else if (_targetDetector.Detected)
        {
            _agent.SetDestination(transform.position);
            _animator.SetFloat(Speed, 0);
            _animator.SetTrigger(Attack);
            _gameController.Lose();
        }
        else
        {
            _animator.SetFloat(Speed, _agent.velocity.magnitude);
            PathProcessing();
        }
    }

    private bool IsDestinationReached()
    {
        var remainingDistance = _agent.remainingDistance;

        return !float.IsPositiveInfinity(remainingDistance) &&
               _agent.pathStatus == NavMeshPathStatus.PathComplete &&
               Mathf.Approximately(remainingDistance, 0);
    }

    private void SetDestination(int pointIndex)
    {
        _agent.SetDestination(points[pointIndex].position);
    }

    private void PathProcessing()
    {
        if (points.Count < 2) return;
        if (!IsDestinationReached()) return;

        if (_checkInProgress)
        {
            _checkInProgress = false;
            SetDestination(_currentPointIndex);
            return;
        }

        _currentPointIndex += _dir;

        if (loopPath)
        {
            _dir = 1;
            if (_currentPointIndex < 0 || _currentPointIndex >= points.Count)
                _currentPointIndex = 0;
        }
        else
        {
            if (_currentPointIndex >= points.Count)
            {
                _dir = -1;
                _currentPointIndex = points.Count - 2;
            }
            else if (_currentPointIndex < 0)
            {
                _dir = 1;
                _currentPointIndex = 1;
            }
        }

        SetDestination(_currentPointIndex);
    }

    [ContextMenu("CheckPoint")]
    public void CheckPoint()
    {
        if (_checkInProgress) return;

        _checkInProgress = true;
        _agent.SetDestination(tempPoint.position);
    }
}
