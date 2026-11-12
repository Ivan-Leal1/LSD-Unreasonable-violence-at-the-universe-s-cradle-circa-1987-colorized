using UnityEngine;

public class ItemEyePoison : ItemBase
{
    [SerializeField] string _selfDescription;
    [SerializeField] Sprite _selfImage;

    public override void OnUse() {
        GameManager.Singleton.ChangeValue(1, 0); //increase damage
        GameManager.Singleton.ChangeValue(-5, 1); //decrease accuracy
    }

    public override string GetDesc() {
        return _selfDescription;
    }

    public override Sprite GetImg() {
        return _selfImage;
    }
}
