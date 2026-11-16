using UnityEngine;

public class ItemMedkit : ItemBase
{
    [SerializeField] string _selfDescription;
    [SerializeField] Sprite _selfImage;

    public override void OnUse() {
        GameManager.Singleton.ChangeHp(4);
    }

    public override string GetDesc() {
        return _selfDescription;
    }

    public override Sprite GetImg() {
        return _selfImage;
    }
}
