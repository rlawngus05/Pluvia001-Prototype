using System.Collections;
using UnityEditor.Search;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DartTrap : TrapLogic
{
    [SerializeField] private GameObject _dartPrefab;
    [SerializeField] private GameObject _dartDestoryer;
    [SerializeField] private float _shootInterval;
    [SerializeField] private bool _isFlip;

    private bool _isPlayerIn;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isPlayerIn = true;
            StartCoroutine(ShootCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { _isPlayerIn = false; }
        
    }

    private IEnumerator ShootCoroutine()
    {
        while (_isPlayerIn)
        {
            GameObject dart = Instantiate(_dartPrefab, transform.position, Quaternion.identity);
            dart.transform.SetParent(gameObject.transform);
            dart.GetComponent<Dart>().StartMove(GiveDamage, _isFlip, _dartDestoryer);

            yield return new WaitForSeconds(_shootInterval);
        }
    }
}
