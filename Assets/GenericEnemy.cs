using UnityEngine;
using System.Collections.Generic;

public class GenericEnemy : EnemyStats
{
    int _hp = 6;
    int _counter = 0;
    [SerializeField] List<string> _speech;
    

    public override void TakeDamage(int damage) {
        _hp -= damage;
        if (_hp <= 0) {
            Debug.Log("Win");
        }
        Debug.Log("enemy hp: " + _hp);
    }

    public override void UseSkill() {
        switch (_counter) {
            case 0:
                //wait
                GameManager.Singleton._textList.Add("Triangle is waiting...");
                break;
            case 1:
                //attack for 1
                GameManager.Singleton.ChangeHp(-1);
                GameManager.Singleton._textList.Add("Triangle attacks!");
                break;
            case 2:
                //heal 1
                TakeDamage(-1);
                GameManager.Singleton._textList.Add("Triangle heals himself!");
                //last skill in order, repeat
                _counter = 0;
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
}
