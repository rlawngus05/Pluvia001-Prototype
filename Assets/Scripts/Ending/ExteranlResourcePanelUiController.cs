using UnityEngine;

public class ExteranlResourcePanelUiController : MonoBehaviour
{
    [SerializeField] GameObject _exteranlResourcePanel;
    [SerializeField] AudioClip _openSoundEffect;
    [SerializeField] AudioClip _closeSoundEffect;

    public void Open() {
        _exteranlResourcePanel.SetActive(true);
        SoundManager.Instance.PlaySoundEffect(_openSoundEffect);
    }
    public void Close() {
        _exteranlResourcePanel.SetActive(false);
        SoundManager.Instance.PlaySoundEffect(_closeSoundEffect);
    }    
}
