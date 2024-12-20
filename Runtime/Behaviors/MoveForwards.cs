using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class MoveForwards : DeepBehavior
    {
        public override void Init()
        {
            parent.mb.SetVelocity(parent.transform.right * parent.attributes[D_Attribute.MoveSpeed].value);
            parent.events.UpdateNorm += Move;
        }

        public override void Teardown()
        {
            parent.events.UpdateNorm -= Move;
        }

        private void Move()
        {
            parent.mb.SetVelocity((parent.mb.velocity / Mathf.Sqrt(parent.mb.velocity.sqrMagnitude)) * parent.attributes[D_Attribute.MoveSpeed].value);
        }
    }
}
