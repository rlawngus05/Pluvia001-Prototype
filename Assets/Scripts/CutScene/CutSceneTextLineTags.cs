using UnityEngine;


public abstract class Tag { }
public abstract class ActionTag : Tag
{
    private int _executeIndex;
    public int ExecuteIndex => _executeIndex;

    public ActionTag(int executeIndex) { _executeIndex = executeIndex; }
}
public class WaitTag : ActionTag
{
    private float _duration;
    public float Duration => _duration;

    public WaitTag(int startIndex, float duration) : base(startIndex)
    {
        _duration = duration;
    }
}

public abstract class StateTag : Tag
{
    private int _startIndex;
    public int StartIndex => _startIndex;
    public int EndIndex { get; set; }

    public StateTag(int startIndex) { _startIndex = startIndex; }
}

public class TypeIntervalTag : StateTag
{
    private float _interval;
    public float Interval => _interval;

    public TypeIntervalTag(int startIndex, float interval) : base(startIndex)
    {
        _interval = interval;
    }
}

public class EffectTag : StateTag
{
    private EffectType _type;
    public EffectType Type => _type;

    public EffectTag(int startIndex, EffectType type) : base(startIndex)
    {
        _type = type;
    }
}

public enum EffectType
{
    Waving,
    Jittering
}
