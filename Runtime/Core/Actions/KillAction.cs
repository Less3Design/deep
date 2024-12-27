using UnityEngine;

namespace Deep
{
    public class KillAction : DeepAction
    {
        public KillAction(DeepEntity target) : base(target, null) { }
        public KillAction(DeepEntity target, DeepEntity source) : base(target, source) { }

        public override void HandleExecute()
        {
            if (target.dying == true) return;

            target.dying = true;

            if (target.type != D_EntityType.Item && target.inventory.items.Count > 0)
            {
                // TODO fix inventory acting as weird array thing.
                /*
                while (target.inventory.items.Count > 0)
                {
                    target.inventory.DropItemIntoWorld(0);
                }
                */
            }

            source?.events.OnKillEntity?.Invoke(target);
            target.events.OnEntityDie?.Invoke();
        }
    }
}
