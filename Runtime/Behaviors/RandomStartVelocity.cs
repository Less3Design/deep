using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class RandomStartVelocity : DeepBehavior
    {
        public override void Init()
        {
            parent.events.OnEntityEnable += Move;
        }

        public override void Teardown()
        {
            parent.events.OnEntityEnable -= Move;
        }

        private void Move()
        {
            Vector2 dir = Random.insideUnitCircle.normalized;

            parent.mb.AddForce(dir * parent.attributes[D_Attribute.MoveSpeed].value);
        }
    }
}