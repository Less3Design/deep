using UnityEngine;

namespace Deep
{
    public class DieOnBounce : DeepBehavior
    {
        public override void Init()
        {
            parent.events.OnBounce += Die;
        }
        public override void Teardown()
        {
            parent.events.OnBounce -= Die;
        }

        private void Die(Vector2 foo)
        {
            new KillAction(parent).Execute();
        }
    }
}
