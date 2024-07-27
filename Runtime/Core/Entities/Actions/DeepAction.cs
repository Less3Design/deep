using Deep;
using UnityEngine;

namespace Deep
{
    /// <summary>
    /// A deep action is a single unit of code that affects the state of a single entity.
    /// Actions can be executed immediatly or batched for later. It doesnt really matter. We don't care about order too much right now.
    /// </summary>
    public abstract class DeepAction
    {
        public DeepEntity target;
        public DeepEntity source;

        // A source entity should always be filled in if possible, but is ultimately optional in most cases.
        public DeepAction(DeepEntity target, DeepEntity source)
        {
            this.target = target;
            this.source = source;
        }

        public DeepAction(DeepEntity target)
        {
            this.target = target;
            this.source = null;
        }

        public abstract void Execute();

        public virtual DeepAction Log()// Used for debugging only
        {
            Debug.Log($"Action [{this.GetType().Name}] => [{(target != null ? target.name : "NULL")}]");
            return this;
        }
    }
}
