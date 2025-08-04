using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class InteractableFxTest : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] protected Material _intertableFxMaterial;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            List<Material> materialsWithFx = new List<Material>(_spriteRenderer.materials);
            materialsWithFx.Add(_intertableFxMaterial);

            _spriteRenderer.materials = materialsWithFx.ToArray();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            List<Material> materialsWithFx = new List<Material>(_spriteRenderer.materials);
            materialsWithFx.RemoveAt(1);

            _spriteRenderer.materials = materialsWithFx.ToArray();
        }
    }
}
