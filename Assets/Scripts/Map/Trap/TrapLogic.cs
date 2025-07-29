using UnityEngine;

abstract public class TrapLogic : MonoBehaviour
{
    [SerializeField] private int damage;

    abstract public void Init();

    virtual public void OnCollisionEnter2D(Collision2D other) {
        HealthManager.Instance.DecreaseHealth(damage);   
    }
}
