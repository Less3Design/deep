using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class AreaBehaviorOnDeath : DeepBehavior
    {
        public float radius;
        public DeepBehavior behavior;
        public D_Team targetTeam;
        public bool applyDublicates;

        public AreaBehaviorOnDeath(float radius, DeepBehavior behavior, D_Team targetTeam, bool applyDuplicates)
        {
            this.radius = radius;
            this.behavior = behavior;
            this.targetTeam = targetTeam;
            this.applyDublicates = applyDuplicates;
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
            DeepUtility.AreaBehavior(parent.transform.position, radius, behavior, owner, targetTeam, applyDublicates);
        }
    }
}