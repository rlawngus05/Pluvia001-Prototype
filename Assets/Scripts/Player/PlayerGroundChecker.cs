using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            PlayerController.Instance.OnGroundAfterJump();
        }
    }
}
