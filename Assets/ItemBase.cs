using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public virtual void OnUse(){ }

    public virtual string GetDesc() {
        return "awa";
    }
}
