using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ActionLineManager : MonoBehaviour
{
    private List<PlayableDirector> _currentScenePlayableDirectors;
    public List<PlayableDirector> CurrentScenePlayableDirectors
    {
        set
        {
            _currentScenePlayableDirectors = value;
        }
    }

    public void Execute(ActionLine actionLine, Action finishLineObserver)
    {
        string playableDirectorId = actionLine.PlayableDirectorId;

        PlayableDirector playableDirector = _currentScenePlayableDirectors.Find((PlayableDirector current) =>
        {
            return current.gameObject.name.ToLower() == playableDirectorId.ToLower();
        });

        if (playableDirector == null)
        {
            throw new Exception($"No PlayableDirector found in this scene for ID \"{playableDirectorId}\"");
        }

        try
        {
            playableDirector.GetComponent<ActionLineFinisher>().SetFinishLineObserver(finishLineObserver);
        }
        catch
        {
            throw new Exception($"Gameobject \"{playableDirectorId}\" doesn't have an \"ActionLineFinsher\" component");
        }

        playableDirector.Play();
    }
}
