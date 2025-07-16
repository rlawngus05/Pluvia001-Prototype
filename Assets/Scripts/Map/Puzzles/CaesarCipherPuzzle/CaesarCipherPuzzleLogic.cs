using System;
using UnityEngine;

public class CaesarCipherPuzzleLogic : PuzzleLogic
{
    [SerializeField] String _plainText;
    [SerializeField] int _n;
    [SerializeField] String _cipherText;

    [ContextMenu(nameof(Init))]
    public override void Init()
    {
        _cipherText = "";

        foreach (char c in _plainText)
        {
            char cipherCharacter = (char)((c - 'A' + _n) % 26 + 'A');

            _cipherText += cipherCharacter;
        }
    }

    public override void CheckCorrection()
    {
    }
}
