using UnityEngine;

[RequireComponent(typeof(TutorialActivator))]
public class ZeroFloorSceneStarter : SceneStarter
{
    [SerializeField] private AudioClip _zeroFloorAmbientBgm;
    private TutorialActivator _tutorialActiavtor;
    
    private void Awake()
    {
        _tutorialActiavtor = GetComponent<TutorialActivator>();
    }

    protected override void Start()
    {
        base.Start();

        _tutorialActiavtor.Activate();
        SoundManager.Instance.PlayBgm(_zeroFloorAmbientBgm);
    }
}
