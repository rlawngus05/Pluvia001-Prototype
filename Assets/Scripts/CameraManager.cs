using Cinemachine;
using UnityEngine;

//* 이 클래스는 씬 싱글톤 이다. 즉, 게임 전체에 걸쳐서 객체가 하나만 존재하는 것이 아닌, 하나의 씬에 오직 하나의 객체만 존재한다는 것이다.
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
