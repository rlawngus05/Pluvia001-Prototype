using UnityEngine;

public interface IInteractable
{
    //* 유저가 F키를 눌렀을 때, 호출할 함수이다.
    //* 상호작용 가능한 모든 물체는 IInteractable를 구현해야 한다.
    public void Interact();
}
