using System.Collections;
using Cinemachine;
using UnityEngine;

//* 이 클래스는 씬 싱글톤 이다. 즉, 게임 전체에 걸쳐서 객체가 하나만 존재하는 것이 아닌, 하나의 씬에 오직 하나의 객체만 존재한다는 것이다.
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    private CinemachineConfiner2D _cameraConfiner;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    [Header("Dead Effect Elements")]
    [SerializeField] private float _deadZoomTime;
    [SerializeField] private float _targetLensOrthoSize;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _cameraConfiner = GetComponent<CinemachineConfiner2D>();
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeConfiner(CompositeCollider2D confiner)
    {
        _cameraConfiner.m_BoundingShape2D = confiner;
    }
    
    //*                     
    //* 사망 효과 관련 코드
    //*

    public IEnumerator ExecuteDeadEffect()
    {
        float originalOrthographicSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;
        float elapsed = .0f;

        while (elapsed <= _deadZoomTime)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / _deadZoomTime;
            _cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(originalOrthographicSize, _targetLensOrthoSize, EasingFunctions.EaseOutCirc(t));

            yield return null;
        }
    }
}
