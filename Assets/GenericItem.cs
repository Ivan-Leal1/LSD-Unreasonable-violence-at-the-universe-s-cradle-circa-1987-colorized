using UnityEngine;

public class GenericItem : ItemBase
{
    public override void OnUse() {
        GameManager.Singleton.ChangeHp(1);
    }
}
