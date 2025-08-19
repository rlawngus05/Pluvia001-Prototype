using UnityEngine;

public class InteractableEther : InteractableItem
{
    public override void Interact()
    {
        EtherManager.Instance.AddEtherCount();

        base.Interact();
    }
}
