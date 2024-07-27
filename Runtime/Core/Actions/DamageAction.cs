using UnityEngine;

namespace Deep
{
    /// <summary>
    /// Apply damage to an entity. Damage can be applied to ANY resource, but Health directly affects the life of the entity.
    /// </summary>
    public class DamageAction : DeepAction
    {
        public Damage[] damage { get; private set; }

        public bool damageNumbers { get; private set; } = true;

        public DamageAction(DeepEntity target, DeepEntity source, params Damage[] damage) : base(target, source)
        {
            this.damage = damage;
        }

        public DamageAction(DeepEntity target, DeepEntity source, Damage damage) : base(target, source)
        {
            this.damage = new[] { damage };
        }

        public DamageAction SetDamageNumbers(bool displayDamageNumbers)
        {
            damageNumbers = displayDamageNumbers;
            return this;
        }

        public override void HandleExecute()
        {
            foreach (Damage d in damage)
            {
                if (d.target == D_Resource.Health)
                {
                    //Shield absorption. Game dependant
                    int sr = target.resources[D_Resource.Shield].Consume(d.damage);
                    int hr = target.resources[D_Resource.Health].Consume(sr);
                    int damageToHP = d.damage - hr;

                    // would rather not eval every time.....
                    if (damageNumbers) DamageNumbers(damageToHP, d.color);

                    target.events.OnTakeDamage?.Invoke(damageToHP);
                    source?.events.OnDealDamage?.Invoke(damageToHP);
                    return;
                }
                target.resources[d.target].Consume(d.damage);
            }
        }

        private void DamageNumbers(int num, Color color)
        {
            // TODO add a hook here. Each game will have its own way of handling damage numbers.
        }
    }
}
