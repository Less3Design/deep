using UnityEngine;

namespace Deep
{
    public class LookAction : DeepAction
    {
        public Vector2 lookDirection { get; private set; }

        public LookAction(DeepEntity target, Vector2 lookDirection) : base(target)
        {
            this.lookDirection = lookDirection;
        }

        public override void HandleExecute()
        {
            target.lookDirection = lookDirection;
            target.events.OnLookDirectionChanged?.Invoke(lookDirection);
        }
    }
}
