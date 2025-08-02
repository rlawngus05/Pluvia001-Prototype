using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorData", menuName = "Scriptable Objects/ActorData")]
public class ActorData : ScriptableObject
{
    [Serializable]
    public struct Portrait
    {
        [SerializeField] private EmotionType _type;
        [SerializeField] private Sprite _sprite;

        public EmotionType Type => _type;
        public Sprite Sprite => _sprite;
    }

    [SerializeField] private string _name;
    [SerializeField] private List<Portrait> _portraits;
    [SerializeField] private AudioClip _typeSoundEffect;

    public string Name => _name;
    public AudioClip TypeSoundEffect => _typeSoundEffect;

    //* 만약 같은 EmotionType의 Portrait이 여러개 있으면 가장 인덱스가 작은 것으로 함
    public Sprite GetPortrait(EmotionType type)
    {
        Sprite sprite = null;

        foreach (Portrait item in _portraits)
        {
            if (item.Type == type)
            {
                sprite = item.Sprite;
                break;
            }
        }

        return sprite;
    }
}

public enum EmotionType
{
    Idle,
    Anger,
    Sad,
    Happy
}
