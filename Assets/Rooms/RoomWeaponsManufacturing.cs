using UnityEngine;

public class RoomWeaponsManufacturing : RoomBase
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
            GameManager.Singleton._textList.Add("You are in a lab");
            GameManager.Singleton._textList.Add("On the floor, you found some gunpowder!");
            GameManager.Singleton._textList.Add("Gunpowder added to your inventory");
            GameManager.Singleton._itemsHeld.Add(_item.GetComponent<ItemBase>());
        } else if (i > 80 && i <= 90) {
            GameManager.Singleton._textList.Add("Everything seems calm");
            GameManager.Singleton._textList.Add("You decide to take a moment to practice your aim");
            GameManager.Singleton._textList.Add("ranged accuracy + 5");
            GameManager.Singleton._rangedAccuracy += 5;
        } else if (i > 90) {
            GameManager.Singleton._textList.Add("Everything seems calm");
            GameManager.Singleton._textList.Add("You decide to take a moment to sharpen your knife");
            GameManager.Singleton._textList.Add("Melee damage +1");
            GameManager.Singleton._meleeDamage += 1;
        }
    }

    public override string Desc() {
        return _description;
    }

    public override Sprite GetImg() {
        return _selfDoor;
    }
}
