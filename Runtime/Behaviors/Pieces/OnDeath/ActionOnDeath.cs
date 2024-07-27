using System;

namespace Deep
{
    public class ActionOnDeath : DeepBehavior
    {
        private Action<DeepEntity>[] actions;
        public ActionOnDeath(params Action<DeepEntity>[] actions)
        {
            this.actions = actions;
        }

        public override void Init()
        {
            parent.events.OnEntityDie += OnDie;
        }

        public override void Teardown()
        {
            parent.events.OnEntityDie -= OnDie;
        }

        private void OnDie()
        {
            foreach (Action<DeepEntity> a in actions)
            {
                a.Invoke(parent);
            }
        }
    }
}