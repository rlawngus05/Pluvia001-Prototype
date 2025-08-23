using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] float _cameraOrthoSize;
    public float CameraOrthoSize => _cameraOrthoSize;
    [SerializeField] private List<Door> doors; 
}
