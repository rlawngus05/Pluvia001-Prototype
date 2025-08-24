using UnityEngine;

public class CutSceneActivator : MonoBehaviour
{
    [SerializeField] private CutSceneScript _cutSceneScript;
    [SerializeField] private bool _isRepeatable;
    private bool _hasActivated;

    void Awake()
    {
        _hasActivated = false;
    }

    public void Activate()
    {
        if (_isRepeatable || !_hasActivated)
        {
            CutSceneManager.Instance.SetScript(_cutSceneScript.Lines);

            CutSceneManager.Instance.StartCutScene();
            _hasActivated = true;
        }
    }
}
