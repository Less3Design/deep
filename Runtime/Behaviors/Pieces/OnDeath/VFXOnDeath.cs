using Deep.VFX;

namespace Deep
{
    public class VFXOnDeath : DeepBehavior
    {
        public DeepVFXAction[] vfxActions;

        public VFXOnDeath(params DeepVFXAction[] vfx)
        {
            vfxActions = vfx;
        }

        public override void Init()
        {
            parent.events.OnEntityDie += OnDeath;
        }

        public override void Teardown()
        {
            parent.events.OnEntityDie -= OnDeath;
        }

        private void OnDeath()
        {
            foreach (DeepVFXAction a in vfxActions)
            {
                a.Execute(parent.transform.position);
            }
        }
    }
}