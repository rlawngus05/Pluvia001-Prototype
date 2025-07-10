using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private float moveSpeed;
    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");
        rigidbody2D.linearVelocityX = moveSpeed * moveDirection;
    }

    public void MoveCharacter(Transform destination)
    {
        transform.position = destination.position;
    }
}
