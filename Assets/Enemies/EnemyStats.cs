using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public virtual void TakeDamage(int damage){ }
    public virtual void UseSkill(){ }
    public virtual void Talk(){ }

    public virtual int CurrentHp() {
        return 1;
    }
}