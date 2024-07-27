using UnityEngine;

namespace Deep
{
    public class HealthObserver : DeepBehavior
    {
        public override void Init()
        {
            parent.resources[D_Resource.Health].onDeplete += HealthDeplete;
        }

        public override void Teardown()
        {
            parent.resources[D_Resource.Health].onDeplete -= HealthDeplete;
        }

        private void HealthDeplete()
        {
            new KillAction(parent).Execute();
        }
    }
}
