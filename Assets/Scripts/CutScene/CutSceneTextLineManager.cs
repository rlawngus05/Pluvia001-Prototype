using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngineInternal;

public class CutSceneTextLineManager : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;

    private List<Coroutine> _coroutines;
    private CutSceneTextLineParser _parser;

    private string _plainText;
    private List<EffectTag> _effectTags;
    private Queue<Tag> _remainNonEffectTags;
    private Stack<Tag> _tagStack;
    private Stack<TypeIntervalTag> _typeIntervalTagStack;
    private Coroutine _typeCoroutine;
    private Action _onTypingEnd;

    [SerializeField] private float defaultJiterringPower;
    [SerializeField] private float defaultWavePhasOffset;
    [SerializeField] private float defaultWaveHeight;
    [SerializeField] private float defaultTypeInterval;

    private void Awake()
    {
        _coroutines = new List<Coroutine>();
        _parser = new CutSceneTextLineParser(defaultJiterringPower, defaultWaveHeight, defaultWavePhasOffset);
        _remainNonEffectTags = new Queue<Tag>();
        _tagStack = new Stack<Tag>();
        _typeIntervalTagStack = new Stack<TypeIntervalTag>();
    }

    private void Update() {
        _textMesh?.ForceMeshUpdate(); // 항상 최신화
    }

    public Coroutine ExecuteLine(string script,TextMeshProUGUI textMeshPro, Action onTypingEnd, AudioClip typeSoundEffect = null)
    {
        foreach (Coroutine coroutine in _coroutines)
        {
            if (coroutine == null) { continue; }
            StopCoroutine(coroutine);
        }

        _coroutines.Clear();
        _onTypingEnd = onTypingEnd;
        _typeIntervalTagStack?.Clear();
        _tagStack?.Clear();
        _effectTags?.Clear();
        _remainNonEffectTags?.Clear();

        _textMesh = textMeshPro;

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
        return _typeCoroutine = StartCoroutine(ApplyTyping(typeSoundEffect));
    }

    private IEnumerator ApplyTyping(AudioClip typeSoundEffect)
    {
        float currentTypeInterval = defaultTypeInterval;
        int plainTextIndex = 0;
        
        for (int i = 0; i < _textMesh.textInfo.characterCount; i++)
        {
            if (_tagStack.Count != 0)
            {
                StateTag stateTag = (StateTag)_tagStack.Peek();

                if (stateTag.EndIndex - 1 == i)
                {
                    if (stateTag is TypeIntervalTag)
                    {
                        if (_typeIntervalTagStack.Count != 0)
                        {
                            _typeIntervalTagStack.Pop();

                            currentTypeInterval = _typeIntervalTagStack.Count == 0 ? defaultTypeInterval : _typeIntervalTagStack.Peek().Interval;
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
                            _typeIntervalTagStack.Push(typeIntervalTag);
                        }
                        _remainNonEffectTags.Dequeue();
                    }
                }
            }

            _textMesh.maxVisibleCharacters = i + 1;

            //* 공백 타이핑 무시
            if (char.IsWhiteSpace(_textMesh.textInfo.characterInfo[i].character)) { continue; }

            //* 공백 제외한 텍스트가 1 + 3의 배수일 때마다 대사 효과음 출력
            if ((plainTextIndex - 1)% 3 == 0 && typeSoundEffect != null)
            {
                SoundManager.Instance.PlaySoundEffectWithRandomPich(typeSoundEffect);
            }
            plainTextIndex++;
            yield return new WaitForSeconds(currentTypeInterval);
        }

        _onTypingEnd();
    }

    public void SkipTyping()
    {
        StopCoroutine(_typeCoroutine);
        _onTypingEnd();
        _textMesh.maxVisibleCharacters = _textMesh.textInfo.characterCount;
    }
    
    private IEnumerator ExecuteActionTag(ActionTag actionTag)
    {
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
                    _coroutines.Add(StartCoroutine(Jittering((JitteringEffectTag)effectTag)));
                    break;
                case EffectType.Waving:
                    _coroutines.Add(StartCoroutine(Waving((WavingEffectTag)effectTag)));
                    break;
            }
        }
    }

    private IEnumerator Jittering(JitteringEffectTag tag)
    {
        TMP_TextInfo textInfo = _textMesh.textInfo;
        int startIndex = tag.StartIndex;
        int endIndex = tag.EndIndex;
        float power = tag.Power;

        while (true)
        {
            Vector3[] vertices = textInfo.meshInfo[0].vertices;

            for (int i = startIndex; i < endIndex; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;
                Vector2 jitterValue = SimpleJitter(power);

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

    private IEnumerator Waving(WavingEffectTag tag)
    {
        TMP_TextInfo textInfo = _textMesh.textInfo;
        int startIndex = tag.StartIndex;
        int endIndex = tag.EndIndex;
        float waveHeight = tag.WaveHeight;
        float phaseOffset = tag.PhaseOffset;

        float elapsed = 0f;

        while (true)
        {
            Vector3[] vertices = textInfo.meshInfo[0].vertices;
            int effectSequence = startIndex;

            for (int i = startIndex; i < endIndex; i++)
            {
                //* 공백에 효과 적용 무시
                if (char.IsWhiteSpace(_textMesh.textInfo.characterInfo[i].character))
                {
                    continue;
                }

                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;
                float yOffset = Mathf.Sin((elapsed + effectSequence++ * phaseOffset) * 2f * Mathf.PI) * waveHeight;

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
