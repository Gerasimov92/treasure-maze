using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    public bool loopPath;
    public List<Transform> points;
    private NavMeshAgent _agent;
    private int _currentPoint = 0;
    private int _dir = 1;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (points.Count >= 2)
            SetDestination(_currentPoint);
    }

    void Update()
    {
        if (points.Count < 2) return;
        if (!IsDestinationReached()) return;

        _currentPoint += _dir;
        if (loopPath)
        {
            _dir = 1;
            if (_currentPoint < 0 || _currentPoint >= points.Count)
                _currentPoint = 0;
        }
        else
        {
            if (_currentPoint >= points.Count)
            {
                _dir = -1;
                _currentPoint = points.Count - 2;
            }
            else if (_currentPoint < 0)
            {
                _dir = 1;
                _currentPoint = 1;
            }
        }

        SetDestination(_currentPoint);
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
}
