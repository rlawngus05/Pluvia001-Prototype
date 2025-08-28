using System;
using System.Collections;
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

        if (actionLine.IsSetter)
        {
            StartCoroutine(ExecuteSetter(playableDirector, finishLineObserver));
        }
        else
        {
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

    private IEnumerator ExecuteSetter(PlayableDirector playableDirector, Action finishLineObserver)
    {
        bool isSet = false;

        yield return ScreenEffectManager.Instance.FadeIn(0.5f);
        playableDirector.stopped += (playableDirector) => { isSet = true; };
        playableDirector.Play();

        yield return new WaitUntil(() => isSet);
        yield return ScreenEffectManager.Instance.FadeOut(0.5f);

        finishLineObserver();
    }
}
