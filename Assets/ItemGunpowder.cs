using UnityEngine;

public class ItemGunpowder : ItemBase
{
    [SerializeField] string _selfDescription;
    [SerializeField] Sprite _selfImage;

    public override void OnUse() {
        GameManager.Singleton.ChangeValue(2, 2); // increase melee damage
    }

    public override string GetDesc() {
        return _selfDescription;
    }

    public override Sprite GetImg() {
        return _selfImage;
    }
}
