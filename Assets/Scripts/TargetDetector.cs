using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    public string targetTag = "Player";
    public int rays = 6;
    public int distance = 15;
    public float angle = 20;
    public Vector3 offset;
    private Transform _target;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag(targetTag).transform;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _target.position) < distance)
        {
            if (Scan())
            {
                // Контакт с целью
            }
            else
            {
                // Поиск цели...
            }
        }
    }

    private bool GetRaycast(Vector3 dir)
    {
        var pos = transform.position + offset;

        if (Physics.Raycast(pos, dir, out var hit, distance))
        {
            if (hit.transform == _target)
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

    private bool Scan()
    {
        var a = false;
        var b = false;
        float currentAngle = 0;

        for (var i = 0; i < rays; i++)
        {
            var x = Mathf.Sin(currentAngle);
            var y = Mathf.Cos(currentAngle);

            currentAngle += angle * Mathf.Deg2Rad / rays;

            var dir = transform.TransformDirection(new Vector3(x, 0, y));
            if(GetRaycast(dir)) a = true;

            if (x != 0)
            {
                dir = transform.TransformDirection(new Vector3(-x, 0, y));
                if(GetRaycast(dir)) b = true;
            }
        }

        return a || b;
    }
}
