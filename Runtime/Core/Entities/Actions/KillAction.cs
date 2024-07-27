using UnityEngine;

namespace Deep
{
    public class KillAction : DeepAction
    {
        public KillAction(DeepEntity target) : base(target, null) { }
        public KillAction(DeepEntity target, DeepEntity source) : base(target, source) { }

        public override void Execute()
        {
            if (target.dying == true) return;

            target.dying = true;

            source?.events.OnKillEntity?.Invoke(target);
            target.events.OnEntityDie.Invoke();
        }
    }
}
