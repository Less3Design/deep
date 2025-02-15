using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class AreaDamageOnDeath : DeepBehavior
    {
        private float radius;
        private Damage damage;
        private D_Team targetTeam;

        public AreaDamageOnDeath(float radius, Damage damage, D_Team targetTeam)
        {
            this.radius = radius;
            this.damage = damage;
            this.targetTeam = targetTeam;
        }

        public override void Init()
        {
            parent.events.OnEntityDie += OnDie;
        }

        public override void Teardown()
        {
            parent.events.OnEntityDie -= OnDie;
        }

        private void OnDie()
        {
            DeepUtility.AreaDamage(parent.transform.position, radius, damage, owner, targetTeam);
        }
    }
}