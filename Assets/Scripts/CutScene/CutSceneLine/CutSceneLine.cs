using System;
using UnityEngine;

[Serializable]
public abstract class CutSceneLine
{
    [SerializeField] private string _lineId;
    public String LineId => _lineId;
}
