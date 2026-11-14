using UnityEngine;

public class RoomBase : MonoBehaviour
{
    public virtual void OnUse(){ }

    public virtual string Desc(){ 
        return "awa";
    }

    public virtual Sprite GetImg() {
        return null;
    }
}
