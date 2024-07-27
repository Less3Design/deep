using UnityEngine;

namespace Deep
{
    /// <summary>
    /// Observes the health resources and kills the entity if it ever reaches 0
    /// </summary>
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
