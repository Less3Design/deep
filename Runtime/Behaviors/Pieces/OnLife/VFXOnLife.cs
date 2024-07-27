using Deep.VFX;
 
namespace Deep
{
    public class VFXOnLife : DeepBehavior
    {
        public DeepVFXAction[] vfxActions;

        public VFXOnLife(params DeepVFXAction[] vfx)
        {
            vfxActions = vfx;
        }

        public override void Init()
        {
            parent.events.OnEntityEnable += OnEnable;
        }

        public override void Teardown()
        {
            parent.events.OnEntityEnable -= OnEnable;
        }

        private void OnEnable()
        {
            foreach (DeepVFXAction a in vfxActions)
            {
                a.Execute(parent.transform.position);
            }
        }
    }
}