using UnityEngine;

public class GenericItem : ItemBase
{
    [SerializeField] string _selfDescription;

    public override void OnUse() {
        GameManager.Singleton.ChangeHp(1);
    }

    public override string GetDesc() {
        return _selfDescription;
    }
}
