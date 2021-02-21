using ProjectStavitski.Combat;
using UnityEngine;

namespace ProjectStavitski.Units
{
    public class Unit : MonoBehaviour, IDamageable
    {
        [Header("Unit Settings")]
        [SerializeField] protected int maxHealth;
        [SerializeField] protected int damage;

        protected float _health;

        protected virtual void Start()
        {
            _health = maxHealth;
        }

        public virtual void TakeDamage(int amount)
        {
            _health = Mathf.Max(_health - amount, 0);

            if (_health <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }

        public virtual void Heal(int amount)
        {
            _health = Mathf.Min(_health + amount, maxHealth);
        }

        public float GetHealth()
        {
            return _health;
        }
        
        public virtual int GetDamage()
        {
            return damage;
        }
    }
}
