using UnityEngine;

public class ItemGunpowder : ItemBase
{
    public override void OnUse() {
        GameManager.Singleton.ChangeValue(2, 2);
    }
}
