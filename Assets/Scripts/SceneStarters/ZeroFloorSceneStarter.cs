using UnityEngine;

[RequireComponent(typeof(CutSceneActivator))]
public class ZeroFloorSceneStarter : SceneStarter
{
    private CutSceneActivator _cutSceneActivator;

    private void Awake() {
        _cutSceneActivator = GetComponent<CutSceneActivator>();
    }

    protected override void Start()
    {
        base.Start();

        _cutSceneActivator.Activate();
    }
}
