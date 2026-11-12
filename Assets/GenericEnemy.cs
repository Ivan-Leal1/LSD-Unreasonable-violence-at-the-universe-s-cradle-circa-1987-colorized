using UnityEngine;

public class GenericEnemy : EnemyStats
{
    int _hp = 6;
    int _counter = 0;

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
                break;
            case 1:
                //attack for 1
                GameManager.Singleton.ChangeHp(-1);
                break;
            case 2:
                //heal 1
                TakeDamage(-1);
                //last skill in order, repeat
                _counter = 0;
                break;
            default:
                break;
        }
        _counter++;
    }

    public override void Talk() {
        Debug.Log("No reason why I'm only doing anything I want to do because I do it all the time");
    }
}
