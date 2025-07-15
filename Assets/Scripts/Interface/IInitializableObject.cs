using UnityEngine;

//* 초기화 작업이 존재한 모든 객체는 IInteractable를 구현해야 한다.
public interface IInitializableObject
{
    public void Init();
}
