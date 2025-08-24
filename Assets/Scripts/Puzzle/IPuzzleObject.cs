using System;
using UnityEngine;

//* 초기화 작업이 존재한 모든 객체는 IInteractable를 구현해야 한다.
//* 이 인터페이스를 소유한 GameObject는 Awake()에서 필드를 초기화하고, Init()에서 로직을 초기화 한다.
public interface IPuzzleObject
{
    public void Initialize();
    public void Initiate();
}