using UnityEngine;

[RequireComponent(typeof(CutSceneActivator))]
public class ZeroFloorSceneStarter : SceneStarter
{
    [SerializeField] private AudioClip _zeroFloorAmbientBgm;
    private CutSceneActivator _cutSceneActivator;
    
    private void Awake()
    {
        _cutSceneActivator = GetComponent<CutSceneActivator>();
    }

    protected override void Start()
    {
        base.Start();

        _cutSceneActivator.Activate();
        SoundManager.Instance.PlayBgm(_zeroFloorAmbientBgm);
    }
}
