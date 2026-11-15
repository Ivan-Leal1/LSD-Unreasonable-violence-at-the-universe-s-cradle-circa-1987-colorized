using UnityEngine;
using System.Collections.Generic;

public class EnemyAbstractShape : EnemyStats
{
    [SerializeField] int _hp;
    int _counter = 0;
    [SerializeField] List<string> _speech;
    [SerializeField] GameObject _itemReward;
    

    public override void TakeDamage(int damage) {
        _hp -= damage;
        if (_hp <= 0) {
            GameManager.Singleton._textList.Add("Abstract shape defeated!");
            GameManager.Singleton._textList.Add("Obtained 1x Poison in the eye");
            GameManager.Singleton._itemsHeld.Add(_itemReward.GetComponent<ItemBase>());
            GameManager.Singleton.Victory();
            Destroy(gameObject);
        }
        Debug.Log("enemy hp: " + _hp);
    }

    public override void UseSkill() {
        switch (_counter) {
            case 0:
                //wait
                GameManager.Singleton._textList.Add("Abstract shape is waiting...");
                break;
            case 1:
                //attack for 1
                GameManager.Singleton._textList.Add("Abstract shape attacks!");
                GameManager.Singleton.ChangeHp(-10);
                break;
            case 2:
                //heal 1
                GameManager.Singleton._textList.Add("Abstract shape heals itself!");
                TakeDamage(-1);
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
        Debug.Log("Instantiated successfuly!");
    }
}