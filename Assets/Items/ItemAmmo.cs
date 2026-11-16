using UnityEngine;

public class ItemAmmo : ItemBase
{
    [SerializeField] string _selfDescription;
    [SerializeField] Sprite _selfImage;

    public override void OnUse() {
        GameManager.Singleton.ChangeValue(999, 4);
    }

    public override string GetDesc() {
        return _selfDescription;
    }

    public override Sprite GetImg() {
        return _selfImage;
    }
}