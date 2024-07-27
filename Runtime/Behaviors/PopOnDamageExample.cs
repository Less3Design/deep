using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class PopOnDamageExample : DeepBehavior
    {
        public float radius;
        public int damage;
        public D_Team targetTeam;

        private DeepViewLink viewRef;
        private float timer;
        private float delay = .25f;
        private bool started;
        private bool finished;//prevents double damage if we kill ourselves

        private Coroutine co;

        public PopOnDamageExample(float radius, int damage, D_Team targetTeam)
        {
            this.radius = radius;
            this.damage = damage;
            this.targetTeam = targetTeam;
        }

        public override void Init()
        {
            parent.events.OnTakeDamage += OnTakeDamage;
            parent.events.OnEntityDie += OnDie;
            viewRef = parent.AddView("PopView");
        }

        public override void Teardown()
        {
            parent.events.OnTakeDamage -= OnTakeDamage;
            parent.events.OnEntityDie -= OnDie;
            viewRef.StartReturn();
        }

        private void OnDie()
        {
            if (started && !finished)
            {
                parent.StopCoroutine(co);
                DeepUtility.AreaDamage(parent.transform.position, radius, new Damage(damage), owner, targetTeam);
                new VFX.CirclePop(Color.red, radius).Execute(parent.transform.position);
                new RemoveBehaviorAction(parent, owner, this).Execute();
            }
        }

        private void OnTakeDamage(float f)
        {
            if (started)
            {
                return;
            }
            started = true;
            co = parent.StartCoroutine(Countdown());
        }
        public IEnumerator Countdown()
        {
            while (timer < delay)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            finished = true;
            DeepUtility.AreaDamage(parent.transform.position, radius, new Damage(damage), owner, targetTeam);
            new VFX.CirclePop(Color.red, radius).Execute(parent.transform.position);
            new RemoveBehaviorAction(parent, owner, this).Execute();
        }
    }
}