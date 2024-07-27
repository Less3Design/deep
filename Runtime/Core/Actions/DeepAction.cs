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
        public bool silent;

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

        public DeepAction Execute()
        {
            this.HandleExecute();
            if (!silent) Log();
            return this;
        }

        public abstract void HandleExecute();

        public DeepAction SetSource(DeepEntity source)
        {
            this.source = source;
            return this;
        }

        private DeepAction Log()// Used for debugging only
        {
            Debug.Log($"Action [{this.GetType().Name}] => [{(target != null ? target.name : "NULL")}]");
            return this;
        }
    }
}
