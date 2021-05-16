using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSectorCreator : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private TargetDetector _targetDetector;
    private float _lastAngle = -1;
    private int _lastDistance = -1;

    void Start()
    {
        _targetDetector = GetComponentInParent<TargetDetector>();
        _meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        var currentAngle = _targetDetector.angle;
        var currentDistance = _targetDetector.distance;

        if (!Mathf.Approximately(currentAngle, _lastAngle) ||
            currentDistance != _lastDistance)
        {
            _meshFilter.mesh = CreateDetectionSectorMesh(currentAngle, currentDistance);
            _lastAngle = currentAngle;
            _lastDistance = currentDistance;
        }
    }

    private Mesh CreateDetectionSectorMesh(float angle, float distance, float step = 10f)
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uvs = new List<Vector2>();

        var right = GetRotation(Vector3.forward, angle) * distance;
        var left = GetRotation(Vector3.forward, angle) * distance;
        var from = left;

        vertices.Add(Vector3.zero);
        vertices.Add(from);
        uvs.Add(Vector2.one * 0.5f);
        uvs.Add(Vector2.one);
        var triangleIdx = 3;

        for (var angleStep = -angle; angleStep < angle; angleStep += step)
        {
            var to = GetRotation(Vector3.forward, angleStep) * distance;
            from = to;
            vertices.Add(from);
            uvs.Add(Vector2.one);
            triangles.Add(triangleIdx - 1);
            triangles.Add(triangleIdx);
            triangles.Add(0);

            triangleIdx++;
        }
        vertices.Add(right);

        uvs.Add(Vector2.one);

        var mesh = new Mesh
        {
            name = "DetectionSector",
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray()
        };
        mesh.RecalculateNormals();

        return mesh;
    }

    private Vector3 GetRotation(Vector3 forward, float angle)
    {
        var rad = angle * Mathf.Deg2Rad;
        var result = new Vector3(forward.x * Mathf.Cos(rad) + forward.z * Mathf.Sin(rad), 0,
            forward.z * Mathf.Cos(rad) - forward.x * Mathf.Sin(rad));
        return result;
    }
}
