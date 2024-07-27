using Deep.VFX;

namespace Deep
{
    public class VFXOnDamage : DeepBehavior
    {
        public DeepVFXAction[] vfxActions;

        public VFXOnDamage(params DeepVFXAction[] vfx)
        {
            vfxActions = vfx;
        }

        public override void Init()
        {
            parent.events.OnTakeDamage += OnDamage;
        }

        public override void Teardown()
        {
            parent.events.OnTakeDamage -= OnDamage;
        }

        private void OnDamage(float damage)
        {
            foreach (DeepVFXAction a in vfxActions)
            {
                a.Execute(parent.transform.position);
            }
        }
    }
}
