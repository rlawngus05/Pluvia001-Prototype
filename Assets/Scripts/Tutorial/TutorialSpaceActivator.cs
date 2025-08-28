using UnityEngine;

public class TutorialSpaceActivator : TutorialActivator
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Activate();
        }
    }
}
