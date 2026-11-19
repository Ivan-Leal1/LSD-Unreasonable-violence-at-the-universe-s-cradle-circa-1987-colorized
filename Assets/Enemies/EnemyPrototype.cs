using UnityEngine;
using System.Collections.Generic;

public class EnemyPrototype : EnemyStats
{
    [SerializeField] int _hp;
    int _counter = 0;
    int _damage = 1;
    [SerializeField] List<string> _speech;
    [SerializeField] GameObject _itemReward;
    

    public override void TakeDamage(int damage) {
        _hp -= damage;
        if (_hp <= 0) {
            GameManager.Singleton._textList.Add("Prototype defeated!");
            GameManager.Singleton._textList.Add("Obtained 1x ammo");
            GameManager.Singleton._itemsHeld.Add(_itemReward.GetComponent<ItemBase>());
            GameManager.Singleton.Victory();
            Destroy(gameObject);
        }
        GameManager.Singleton._statusTexts[1] = "Enemy hp: " + _hp;
        Debug.Log("enemy hp: " + _hp);
    }

    public override void UseSkill() {
        switch (_counter) {
            case 0:
                GameManager.Singleton._textList.Add("Prototype attacks!");
                GameManager.Singleton.ChangeHp(-_damage);
                break;
            case 1:
                GameManager.Singleton._textList.Add("Prototype loses 1 hp. All damage dealt by prototype has been increased by 1");
                TakeDamage(1);
                _damage += 1;
                GameManager.Singleton._statusTexts[2] = "Charge: " + (_damage-1);
                break;
            case 2:
                GameManager.Singleton._textList.Add("Prototype is waiting...");
                //last skill in order, repeat
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

    private void Awake() {
        GameManager.Singleton._statusTexts[1] = "Enemy hp: " + _hp;
    }
}
