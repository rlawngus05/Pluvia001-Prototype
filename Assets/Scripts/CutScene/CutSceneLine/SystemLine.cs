using System;
using UnityEngine;

[Serializable]
public class SystemLine : CutSceneLine
{
    [SerializeField] private string _content;
    public string Content => _content;
}
