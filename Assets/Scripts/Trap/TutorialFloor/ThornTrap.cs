using UnityEngine;

public class ThornTrap : TrapLogic
{
    private bool _isPlayerIn;

    private void Awake()
    {
        _isPlayerIn = false;
    }

    private void Update()
    {
        if (_isPlayerIn) { GiveDamage(); }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { _isPlayerIn = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { _isPlayerIn = false; }
    }
}
