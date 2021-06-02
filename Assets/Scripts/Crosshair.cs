using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;
    private CinemachineVirtualCamera _vcam;
    void Start()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        crosshair.SetActive(_vcam.Priority > 100);
    }
}
