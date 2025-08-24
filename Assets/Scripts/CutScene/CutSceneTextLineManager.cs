using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngineInternal;

public class CutSceneTextLineManager : MonoBehaviour
{
    [SerializeField] private string _originalText;
    [SerializeField] private TextMeshProUGUI _textMesh;

    private List<Coroutine> _coroutines;
    private CutSceneTextLineParser _parser;

    private string _plainText;
    private List<EffectTag> _effectTags;
    private Queue<Tag> _remainNonEffectTags;
    private Stack<Tag> _tagStack;

    [SerializeField] private float jiterringPower;
    [SerializeField] private float characterInterval = 0.2f; // 위상 차이
    [SerializeField] private float maxHeight = 5f;          // 파도 높이
    [SerializeField] private float defaultTypeInterval;

    private void Awake()
    {
        _parser = new CutSceneTextLineParser();
        _coroutines = new List<Coroutine>(); // ✅ 초기화
        _remainNonEffectTags = new Queue<Tag>();
        _tagStack = new Stack<Tag>();

        ExecuteLine(_originalText);
    }

    private void Update() {
        _textMesh?.ForceMeshUpdate(); // 항상 최신화
    }

    public void ExecuteLine(string script)
    {
        (string plainText, List<EffectTag> effectTags, List<Tag> nonEffectTags) parseResult = _parser.Parse(script);

        _plainText = parseResult.plainText;
        _effectTags = parseResult.effectTags;
        _remainNonEffectTags = new Queue<Tag>(parseResult.nonEffectTags);

        _textMesh.text = _plainText;
        _textMesh.ForceMeshUpdate();

        TMP_TextInfo textInfo = _textMesh.textInfo;

        _textMesh.maxVisibleCharacters = 0;

        _textMesh.ForceMeshUpdate();

        ApplyEffectTag();
        StartCoroutine(ApplyTyping());
    }

    private Stack<TypeIntervalTag> typeIntervalTagStack = new Stack<TypeIntervalTag>();
    private IEnumerator ApplyTyping()
    {
        float currentTypeInterval = defaultTypeInterval;

        for (int i = 0; i < _textMesh.text.Length; i++)
        {
            if (_tagStack.Count != 0)
            {
                StateTag stateTag = (StateTag)_tagStack.Peek();

                if (stateTag.EndIndex - 1 == i)
                {
                    if (stateTag is TypeIntervalTag)
                    {
                        if (typeIntervalTagStack.Count != 0)
                        {
                            typeIntervalTagStack.Pop();

                            currentTypeInterval = typeIntervalTagStack.Count == 0 ? defaultTypeInterval : typeIntervalTagStack.Peek().Interval;
                        }
                    }

                    _tagStack.Pop();
                }
            }

            if (_remainNonEffectTags.Count != 0)
            {
                if (_remainNonEffectTags.Peek() is ActionTag actionTag)
                {
                    if (actionTag.ExecuteIndex == i)
                    {
                        yield return StartCoroutine(ExecuteActionTag(actionTag));
                        _remainNonEffectTags.Dequeue();
                    }
                }

                if (_remainNonEffectTags.Peek() is StateTag stateTag)
                {
                    if (stateTag.StartIndex == i)
                    {
                        if (stateTag is TypeIntervalTag typeIntervalTag)
                        {
                            currentTypeInterval = typeIntervalTag.Interval;

                            _tagStack.Push(typeIntervalTag);
                            typeIntervalTagStack.Push(typeIntervalTag);
                        }
                        _remainNonEffectTags.Dequeue();
                    }
                }
            }

            yield return new WaitForSeconds(currentTypeInterval);
            _textMesh.maxVisibleCharacters = i + 1;
        }
    }

    private IEnumerator ExecuteActionTag(ActionTag actionTag) {
        if (actionTag is WaitTag waitTag)
        {
            yield return new WaitForSeconds(waitTag.Duration);
        }
    }

    private void ApplyEffectTag()
    {
        foreach (EffectTag effectTag in _effectTags)
        {
            switch (effectTag.Type)
            {
                case EffectType.Jittering:
                    _coroutines.Add(StartCoroutine(Jittering(effectTag.StartIndex, effectTag.EndIndex)));
                    break;
                case EffectType.Waving:
                    _coroutines.Add(StartCoroutine(Waving(effectTag.StartIndex, effectTag.EndIndex)));
                    break;
            }
        }
    }

    private IEnumerator Jittering(int startIndex, int endIndex)
    {
        TMP_TextInfo textInfo = _textMesh.textInfo;

        while (true)
        {
            Vector3[] vertices = textInfo.meshInfo[0].vertices;

            for (int i = startIndex; i < endIndex; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;
                Vector2 jitterValue = SimpleJitter(jiterringPower);

                for (int j = 0; j < 4; j++)
                {
                    vertices[vertexIndex + j] += (Vector3)jitterValue;
                }
            }

            // ✅ 반드시 meshInfo[0].mesh에 반영
            textInfo.meshInfo[0].mesh.vertices = vertices;
            _textMesh.UpdateGeometry(textInfo.meshInfo[0].mesh, 0);

            yield return null;
        }
    }

    private IEnumerator Waving(int startIndex, int endIndex)
    {
        TMP_TextInfo textInfo = _textMesh.textInfo;
        float elapsed = 0f;

        while (true)
        {
            Vector3[] vertices = textInfo.meshInfo[0].vertices;

            for (int i = startIndex; i < endIndex; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;
                float yOffset = Mathf.Sin((elapsed + i * characterInterval) * 2f * Mathf.PI) * maxHeight;

                for (int j = 0; j < 4; j++)
                {
                    vertices[vertexIndex + j].y += yOffset;
                }
            }

            textInfo.meshInfo[0].mesh.vertices = vertices;
            _textMesh.UpdateGeometry(textInfo.meshInfo[0].mesh, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private Vector2 SimpleJitter(float intensity, System.Random rng = null)
    {
        if (rng == null) rng = new System.Random();
        float x = (float)(rng.NextDouble() * 2.0 - 1.0);
        float y = (float)(rng.NextDouble() * 2.0 - 1.0);
        return new Vector2(x, y) * intensity;
    }
}
