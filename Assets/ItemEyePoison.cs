using UnityEngine;

public class ItemEyePoison : ItemBase
{
    public override void OnUse() {
        GameManager.Singleton.ChangeValue(1, 0); //increase damage
        GameManager.Singleton.ChangeValue(-5, 1); //decrease accuracy
    }
}
