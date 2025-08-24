using System;
using UnityEngine;
using UnityEngine.Playables;

public class ActionLineManager : MonoBehaviour
{
    public void Execute(ActionLine actionLine, Action finishLineObserver)
    {
        GameObject timeLinePrefab = actionLine.TimeLinePrefab;

        GameObject timeLineGameObject = Instantiate(timeLinePrefab);
        timeLineGameObject.transform.SetParent(transform);

        PlayableDirector actionDirector = timeLineGameObject.GetComponent<PlayableDirector>();
        timeLineGameObject.GetComponent<ActionLineFinisher>().SetFinishLineObserver(finishLineObserver);

        actionDirector.Play();
    }
}
