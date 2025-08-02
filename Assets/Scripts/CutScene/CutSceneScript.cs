using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CutSceneScript", menuName = "Scriptable Objects/CutSceneScript")]
public class CutSceneScript : ScriptableObject
{
    [SerializeReference, SubclassSelector] private List<CutSceneLine> _lines;
    public List<CutSceneLine> Lines => _lines;
}
