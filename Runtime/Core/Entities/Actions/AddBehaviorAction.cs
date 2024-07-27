using UnityEngine;

namespace Deep
{
    public class AddBehaviorAction : DeepAction
    {
        public DeepBehavior behavior { get; private set; }

        public AddBehaviorAction(DeepEntity target, DeepEntity source, DeepBehavior behavior) : base(target, source)
        {
            this.behavior = behavior;
        }

        public override void HandleExecute()
        {
            behavior.parent = target;
            behavior.owner = source;
            target.behaviors.Add(behavior);
            behavior.Init();
            if (behavior is DeepAbility a)
            {
                target.abilities.Add(a);
            }
        }

        public RemoveBehaviorAction CreateRemoveAction(DeepEntity source)
        {
            return new RemoveBehaviorAction(target, source, behavior);
        }
    }
}
