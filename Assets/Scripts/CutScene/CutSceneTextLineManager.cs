using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;


public class CutSceneTextLineManager : MonoBehaviour
{
    private static readonly string _commandPattern = @"(<[^>]+>|[^<]+)";

    private string _plainText;
    private List<Tag> _tags;
    [SerializeField] private string _originalText;

    private void Awake()
    {
        _tags = new List<Tag>();

        Execute(_originalText);
    }

    public void Execute(string script)
    {
        _plainText = "";
        _tags.Clear();

        Parse(script);

        Debug.Log(_plainText);
        foreach (Tag tag in _tags)
        {
            if (tag is WaitTag waitTag)
            {
                Debug.LogFormat("WaitTag : {0}", waitTag.ExecuteIndex);
            }
            else if (tag is TypeIntervalTag typeIntervalTag)
            {
                Debug.LogFormat("TypeIntervalTag : {0}~{1}", typeIntervalTag.StartIndex, typeIntervalTag.EndIndex);
            }
            else if (tag is EffectTag effectTag)
            {
                Debug.LogFormat("EffectTag : {0}~{1}", effectTag.StartIndex, effectTag.EndIndex);
            }
            else
            {
                Debug.Log("시발 뭔데");
            }
        }
    }

    private void Parse(string script)
    {
        Stack<Tag> tagStack = new Stack<Tag>();
        int currentIndex = 0;

        MatchCollection matches = Regex.Matches(script, _commandPattern);

        foreach (Match match in matches)
        {
            string value = match.Value;
            if (string.IsNullOrEmpty(value.Trim())) continue;

            if (value.StartsWith("<") && value.EndsWith(">"))
            {
                // 태그 처리
                ParseTag(value, tagStack, currentIndex);
            }
            else
            {
                // 일반 텍스트 처리
                _plainText += value;
                currentIndex += value.Length; // 텍스트 길이만큼 인덱스 증가
            }
        }
        
        if (tagStack.Count > 0)
        {
            Debug.LogWarning("Parsing finished but some effect tags were not closed. Automatically closing them.");
            // 스택에 남은 태그들을 강제로 닫아줌
            while(tagStack.Count > 0)
            {
                StateTag openTag = (StateTag)tagStack.Pop();
                openTag.EndIndex = currentIndex;
            }
        }
    }

    private void ParseTag(string tag, Stack<Tag> tagStack, int currentIndex)
    {
        string tagContent = tag.Substring(1, tag.Length - 2).Trim();
        bool isClosingTag = tagContent.StartsWith("/");
        if (isClosingTag)
        {
            tagContent = tagContent.Substring(1);
        }

        string[] splitedTag = tagContent.Split(' ');
        string tagName = splitedTag[0].ToLower();


        if (tagName == "wait")
        {
            if (isClosingTag)
            {
                throw new System.Exception("병신 포맷 안 맞음");
            }

            WaitTag waitTag = new WaitTag(currentIndex, 1.0f);
            _tags.Add(waitTag);
        }
        else
        {
            switch (tagName)
            {
                case "typeinterval":
                    if (isClosingTag)
                    {
                        Tag top = tagStack.Pop();

                        if (top is TypeIntervalTag typeIntervalTag)
                        {
                            typeIntervalTag.EndIndex = currentIndex;
                        }
                        else
                        {
                            throw new System.Exception("시발년아 포맷 안 맞아");
                        }
                    }
                    else
                    {
                        TypeIntervalTag typeIntervalTag = new TypeIntervalTag(currentIndex, 1.0f);
                        _tags.Add(typeIntervalTag);
                        tagStack.Push(typeIntervalTag);
                    }
                    break;

                case "effect":
                    if (isClosingTag)
                    {
                        Tag top = tagStack.Pop();

                        if (top is EffectTag effectTag)
                        {
                            effectTag.EndIndex = currentIndex;
                        }
                        else
                        {
                            throw new System.Exception("시발년아 포맷 안 맞아");
                        }
                    }
                    else
                    {
                        EffectTag typeIntervalTag = new EffectTag(currentIndex, EffectType.Waving);
                        _tags.Add(typeIntervalTag);
                        tagStack.Push(typeIntervalTag);
                    }
                    break;
            }
        }
    }
}

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
