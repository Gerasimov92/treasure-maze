using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    public string targetTag = "Player";
    public int rayCount = 6;
    public int maxDistance = 15;
    public float angle = 40;
    public Vector3 offset;

    private ITarget _target;
    private Transform _targetTransform;

    void Start()
    {
        var obj = GameObject.FindGameObjectWithTag(targetTag);
        _target = obj.GetComponent<ITarget>();
        _targetTransform = obj.transform;
    }

    void FixedUpdate()
    {
        var scaledDistance = _target.VisualDetectionDistance(maxDistance);
        if (Vector3.Distance(transform.position, _targetTransform.position) > scaledDistance)
            return;

        if (Scan(scaledDistance))
        {
            // Цель в поле зрения
        }
        else
        {
            // Цель не обнаружена
        }
    }

    private bool GetRaycast(Vector3 dir, float distance)
    {
        var pos = transform.position + offset;

        if (Physics.Raycast(pos, dir, out var hit, distance))
        {
            if (hit.transform == _targetTransform)
            {
                Debug.DrawLine(pos, hit.point, Color.green);
                return true;
            }

            Debug.DrawLine(pos, hit.point, Color.blue);
        }
        else
        {
            Debug.DrawRay(pos, dir * distance, Color.red);
        }

        return false;
    }

    private bool Scan(float distance)
    {
        var a = false;
        var b = false;
        float currentAngle = 0;

        for (var i = 0; i < rayCount; i++)
        {
            var x = Mathf.Sin(currentAngle);
            var y = Mathf.Cos(currentAngle);

            currentAngle += angle / 2 * Mathf.Deg2Rad / rayCount;

            var dir = transform.TransformDirection(new Vector3(x, 0, y));
            if(GetRaycast(dir, distance)) a = true;

            if (x != 0)
            {
                dir = transform.TransformDirection(new Vector3(-x, 0, y));
                if(GetRaycast(dir, distance)) b = true;
            }
        }

        return a || b;
    }
}
