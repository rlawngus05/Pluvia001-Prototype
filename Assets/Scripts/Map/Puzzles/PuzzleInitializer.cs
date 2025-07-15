using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//* Puzzle 내의 요소간의 (Ex_ UI, Logic) 초기화 순서 문제를 해결하기 위한 용도로 존재하는 클래스이다.
public class PuzzleInitializer : MonoBehaviour
{
    //* 유니티 에디터에서 Interface를 등록할 수 없어서, 게임 오브젝트를 통해 간접적으로 접근함.
    [SerializeField] List<GameObject> obectsWithIntializable;
    void Start()
    {
        //* Unity Editor 상에서 등록한 객체(GameObject)의 순서에 따라 초기화 함
        foreach (GameObject gameObject in obectsWithIntializable)
        {
            IInitializableObject intializableObject = gameObject.GetComponent<IInitializableObject>();

            if (intializableObject == null)
            {
                Debug.LogError(gameObject.name + " does not have IInitializableObject"); 
                continue;
            }

            intializableObject.Init();
        }
    }
}
