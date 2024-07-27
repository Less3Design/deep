using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class AreaImpulseOnDeath : DeepBehavior
    {
        private float radius;
        private float strength;
        private D_Team targetTeam;

        public AreaImpulseOnDeath(float radius, float strength, D_Team targetTeam)
        {
            this.radius = radius;
            this.strength = strength;
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
            DeepUtility.AreaImpulse(parent.transform.position, radius, strength, targetTeam);
        }
    }
}