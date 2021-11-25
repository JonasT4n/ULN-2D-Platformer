using UnityEngine;

public abstract class MakhlukHidup : MonoBehaviour
{
    [SerializeField]
    protected float health = 0f;
    [SerializeField]
    protected float defense = 0f;
    [SerializeField]
    protected float damage = 0f;

    /// <summary>
    /// ~~ Kasus ~~
    /// Player: HP = 300; DEF = 50; ATT = 30;
    /// Enemy: HP = 50; DEF = 30; ATT = 30;
    /// Rumus Kena Damage: 70 % dari selisih antara Damage dan Defense
    /// 
    /// Note: Minimal Damage = 1;
    /// 
    /// Contoh: Player Pukul Enemy
    /// Damage Player = 70% x (30 - 30) = 0 --> 1;
    /// Damage Enemy = 70% x (30 - 50) = 0 --> 1;
    /// </summary>
    public virtual void GetDamage(float damage)
    {
        float resDmg = Mathf.Max(1f, 7f / 10f * (defense - damage));
        health -= resDmg;

        // Check if entity dead
        if (health <= 0f)
        {
            health = 0f;
            Die();
        }
    }

    public abstract void Die();

    protected abstract void MeleeAttack();
}
