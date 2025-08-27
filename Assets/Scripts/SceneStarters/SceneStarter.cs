using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneStarter : MonoBehaviour
{
    [SerializeField] List<PlayableDirector> _scenePlayableDirectors;
    
    protected virtual void Start()
    {
        CutSceneManager.Instance.SetCurrentScenePlayableDirectors(_scenePlayableDirectors);
    }
}
