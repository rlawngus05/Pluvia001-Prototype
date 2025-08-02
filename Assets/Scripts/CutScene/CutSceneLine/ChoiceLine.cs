using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class ChoiceLine : CutSceneLine
{
    [Serializable]
    public struct ChoiceEvent
    {
        [SerializeField] private string _content;
        [SerializeField] private string _targetEventId;

        public string Content => _content;
        public string TargetEventId => _targetEventId;
    };

    [SerializeField] private List<ChoiceEvent> _choiceEvents;
    public List<ChoiceEvent> ChoiceEvents => _choiceEvents;
}
