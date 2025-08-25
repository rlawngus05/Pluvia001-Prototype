using System;
using UnityEngine;

[Serializable]
public class SystemLine : CutSceneLine
{
    [SerializeField,  TextArea(3, 10)] private string _content;
    public string Content => _content;
}
