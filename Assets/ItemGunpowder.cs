using UnityEngine;

public class ItemGunpowder : ItemBase
{
    [SerializeField] string _selfDescription;

    public override void OnUse() {
        GameManager.Singleton.ChangeValue(2, 2);
    }

    public override string GetDesc() {
        return _selfDescription;
    }
}
