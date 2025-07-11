using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    private CinemachineConfiner2D cameraConfiner;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            cameraConfiner = GetComponent<CinemachineConfiner2D>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeConfiner(CompositeCollider2D confiner)
    {
        cameraConfiner.m_BoundingShape2D = confiner;
    }
}
