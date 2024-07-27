using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class DamageToCollisionOverTime : DeepBehavior
    {
        public Damage damage;
        public float timeBetweenTicks;
        public bool tickImmediatly;
        public D_Team targetTeam;
        public D_EntityType targetType;

        private float timer;

        public DamageToCollisionOverTime(Damage damage, float timeBetweenTicks, bool tickImmediatly, D_Team targetTeam, D_EntityType targetType)
        {
            this.damage = damage;
            this.timeBetweenTicks = timeBetweenTicks;
            this.tickImmediatly = tickImmediatly;
            this.targetTeam = targetTeam;
            this.targetType = targetType;
        }

        public override void Init()
        {
            if (tickImmediatly)
            {
                Tick();
            }
            parent.events.UpdateNorm += Update;
        }

        public override void Teardown()
        {
            parent.events.UpdateNorm -= Update;
        }

        private void Tick()
        {
            foreach (DeepEntity e in parent.activeCollisions.Values)
            {
                if (e.team == targetTeam && e.type == targetType)
                {
                    new DamageAction(e, owner, damage).Execute();
                }
            }
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenTicks)
            {
                timer -= timeBetweenTicks;
                Tick();
            }
        }
    }
}