using UnityEngine;

public class ItemFracturedMoment : ItemBase
{
    [SerializeField] string _selfDescription;
    [SerializeField] Sprite _selfImage;

    public override void OnUse() {
        GameManager.Singleton.ChangeValue(-5, 3);
        GameManager.Singleton.ChangeValue(5, 1);
    }

    public override string GetDesc() {
        return _selfDescription;
    }

    public override Sprite GetImg() {
        return _selfImage;
    }
}
