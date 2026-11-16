using UnityEngine;
using System.Collections.Generic;

public class EnemyWindow : EnemyStats
{
    [SerializeField] int _hp;
    int _counter = 0;
    [SerializeField] List<string> _speech;
    [SerializeField] GameObject _itemReward;
    

    public override void TakeDamage(int damage) {
        _hp -= damage;
        if (_hp <= 0) {
            GameManager.Singleton._textList.Add("Thing in the window defeated!");
            GameManager.Singleton._textList.Add("Obtained 1x fractured moment");
            GameManager.Singleton._itemsHeld.Add(_itemReward.GetComponent<ItemBase>());
            GameManager.Singleton.Victory();
            Destroy(gameObject);
        }
        Debug.Log("enemy hp: " + _hp);
    }

    public override void UseSkill() {
        switch (_counter) {
            case 0:
                GameManager.Singleton._textList.Add("5");
                break;
            case 1:
                GameManager.Singleton._textList.Add("4");
                break;
            case 2:
                GameManager.Singleton._textList.Add("3");
                break;
            case 3:
                GameManager.Singleton._textList.Add("2");
                break;
            case 4:
                GameManager.Singleton._textList.Add("1");
                break;
            case 5:
                GameManager.Singleton._textList.Add("...");
                GameManager.Singleton._textList.Add("Goodbye.");
                GameManager.Singleton._textList.Add("Thing in the window attacks!");
                GameManager.Singleton.ChangeHp(-10);
                _counter = -1;
                break;
            default:
                break;
        }
        _counter++;
    }

    public override void Talk() {
        for (int i = 0; i < _speech.Count; i++) {
            GameManager.Singleton._textList.Add(_speech[i]);
        }
    }

    public override int CurrentHp() {
        return _hp;
    }
}
