using UnityEngine;

public class CutSceneActivator : MonoBehaviour
{
    [SerializeField] private CutSceneScript _cutSceneScript;

    //! Test
    private void Start()
    {
        Activate();
    }

    public void Activate()
    {
        CutSceneManager.Instance.SetScript(_cutSceneScript.Lines);

        CutSceneManager.Instance.StartCutScene();
    }
}
