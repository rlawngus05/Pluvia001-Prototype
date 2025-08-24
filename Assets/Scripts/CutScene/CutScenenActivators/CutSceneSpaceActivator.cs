using UnityEngine;

public class CutSceneSpaceActivator : CutSceneActivator
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Activate();
    }
}
