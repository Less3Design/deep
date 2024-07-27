using UnityEngine;

namespace Deep
{
    public class RemoveBehaviorAction : DeepAction
    {
        public DeepBehavior behavior { get; private set; }

        public RemoveBehaviorAction(DeepEntity target, DeepEntity source, DeepBehavior beahvior) : base(target, source)
        {
            this.behavior = beahvior;
        }

        public override void HandleExecute()
        {
            if (behavior == null || !target.behaviors.Contains(behavior))
            {
                Debug.LogWarning("Remove Behavior Action executed but behavoir was not found on entity");
            }

            behavior.Teardown();

            if (behavior is DeepAbility ability)
            {
                target.abilities.Remove(ability);
            }

            target.behaviors.Remove(behavior);
        }
    }
}
