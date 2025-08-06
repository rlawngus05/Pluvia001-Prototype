using UnityEngine;

abstract public class TrapLogic : MonoBehaviour
{
    [SerializeField] private int _damage;
    public void GiveDamage() { HealthManager.Instance.DecreaseHealth(_damage); } 
    public void GiveDamage(int damage) { HealthManager.Instance.DecreaseHealth(damage);  }
}
