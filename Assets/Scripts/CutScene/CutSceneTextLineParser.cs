using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CutSceneTextLineParser
{
    private static readonly string _tagPattern = @"(<[^>]+>|[^<]+)";

    private string _plainText;
    private List<EffectTag> _effectTags;
    private List<Tag> _nonEffectTags;

    public CutSceneTextLineParser()
    {
        _effectTags = new List<EffectTag>();
        _nonEffectTags = new List<Tag>();
    }

    // public void Execute(string script)
    // {
    //     _plainText = "";
    //     _tags.Clear();

    //     Parse(script);

    //     Debug.Log(_plainText);
    //     foreach (Tag tag in _tags)
    //     {
    //         if (tag is WaitTag waitTag)
    //         {
    //             Debug.LogFormat("WaitTag : {0}, time : {1}", waitTag.ExecuteIndex, waitTag.Duration);
    //         }
    //         else if (tag is TypeIntervalTag typeIntervalTag)
    //         {
    //             Debug.LogFormat("TypeIntervalTag : {0}~{1}, typeInterval : {2}", typeIntervalTag.StartIndex, typeIntervalTag.EndIndex, typeIntervalTag.Interval);
    //         }
    //         else if (tag is EffectTag effectTag)
    //         {
    //             Debug.LogFormat("EffectTag : {0}~{1}, type : {2}", effectTag.StartIndex, effectTag.EndIndex, effectTag.Type);
    //         }
    //         else
    //         {
    //             Debug.Log("시발 뭔데");
    //         }
    //     }
    // }

    public (string palinText, List<EffectTag> effectTags, List<Tag> nonEffectTags) Parse(string script)
    {
        _plainText = "";
        _effectTags.Clear();
        _nonEffectTags.Clear();

        Stack<Tag> tagStack = new Stack<Tag>();
        int currentIndex = 0;

        MatchCollection matches = Regex.Matches(script, _tagPattern);

        foreach (Match match in matches)
        {
            string value = match.Value;
            if (string.IsNullOrEmpty(value.Trim())) continue;

            if (value.StartsWith("<") && value.EndsWith(">"))
            {
                // 태그 처리
                currentIndex = ParseTag(value, tagStack, currentIndex);
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
            while (tagStack.Count > 0)
            {
                StateTag openTag = (StateTag)tagStack.Pop();
                openTag.EndIndex = currentIndex;
            }
        }

        return (_plainText, _effectTags, _nonEffectTags);
    }

    private int ParseTag(string tag, Stack<Tag> tagStack, int currentIndex)
    {
        string tagContent = tag.Substring(1, tag.Length - 2).Trim();
        bool isClosingTag = tagContent.StartsWith("/");
        if (isClosingTag)
        {
            tagContent = tagContent.Substring(1);
        }

        string[] splitedTag = tagContent.Split(' ');
        string tagName = splitedTag[0].ToLower();

        Dictionary<string, string> attributes = new Dictionary<string, string>();
        if (!isClosingTag)
        {
            for (int i = 1; i < splitedTag.Length; i++)
            {
                string attribute = splitedTag[i];
                string[] splited = attribute.Split('=');

                string attributeName = splited[0].ToLower();
                string attributeValue = splited[1];

                attributes.Add(attributeName, attributeValue);
            }
        }

        if (tagName == "wait")
        {
            if (isClosingTag)
            {
                throw new Exception("<Wait> tag doesn't need Closing Tag");
            }

            WaitTag waitTag;
            try
            {
                float time = float.Parse(attributes["duration"]);
                waitTag = new WaitTag(currentIndex, time);
            }
            catch (KeyNotFoundException)
            {
                throw new System.Exception("There is no \'Duration\' attribute in the <Wait> tag");
            }

            _nonEffectTags.Add(waitTag);
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
                            throw new Exception("The format is wrong");
                        }
                    }
                    else
                    {
                        TypeIntervalTag typeIntervalTag;
                        try
                        {
                            float interval = float.Parse(attributes["interval"]);
                            typeIntervalTag = new TypeIntervalTag(currentIndex, interval);
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new Exception("There is no \'Interval\' attribute in the <TypeInterval> tag");
                        }

                        _nonEffectTags.Add(typeIntervalTag);
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
                            throw new Exception("The format is wrong");
                        }
                    }
                    else
                    {
                        EffectTag effectTag;
                        EffectType effectType;
                        try
                        {
                            effectType = (EffectType)Enum.Parse(typeof(EffectType), attributes["type"], true);
                            effectTag = new EffectTag(currentIndex, effectType);
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new Exception("There is no \'Type\' attribute in the <Effect> tag");
                        }
                        catch (ArgumentException)
                        {
                            throw new Exception($"There is no type for '{attributes["type"]}'");
                        }

                        _effectTags.Add(effectTag);
                        tagStack.Push(effectTag);
                    }
                    break;

                default: //* 유효하지 않은 태그명이면 일반 텍스트로 삽입함
                    _plainText += tag;
                    currentIndex += tag.Length;
                    break;
            }
        }

        return currentIndex;
    }
}
