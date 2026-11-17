using UnityEngine;
using UnityEngine.UI;

public class RoomBreak : RoomBase
{
    [SerializeField] GameObject _encounter;
    [SerializeField] GameObject _item;
    [SerializeField] string _description;
    [SerializeField] Sprite _selfDoor;

    public override void OnUse() {
        int i = Random.Range(0, 100);
        if (i <= 50) {
            Debug.Log("Encounter");
            GameManager.Singleton.ReceiveEnemy(_encounter);
        } else if (i > 50 && i <= 80) {
            Debug.Log("Event");
            GameManager.Singleton._textList.Add("You are in the employee break room");
            GameManager.Singleton._textList.Add("Stuck to the wall, you find a medkit!");
            GameManager.Singleton._textList.Add("Medkit added to your inventory");
            GameManager.Singleton._itemsHeld.Add(_item.GetComponent<ItemBase>());
        } else if (i > 80) {
            Debug.Log("Secret third thing");
            GameManager.Singleton._textList.Add("Everything seems calm");
            GameManager.Singleton._textList.Add("You decide to take a moment to rest");
            GameManager.Singleton._textList.Add("Healed 2 hp");
            GameManager.Singleton.ChangeHp(2);
        }
    }

    public override string Desc() {
        return _description;
    }

    public override Sprite GetImg() {
        return _selfDoor;
    } 
}
