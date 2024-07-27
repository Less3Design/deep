using UnityEngine;
using Deep.VFX;

namespace Deep
{
    public class VFXOnBounce : DeepBehavior
    {
        public DeepVFXAction[] vfxActions;

        public VFXOnBounce(params DeepVFXAction[] vfx)
        {
            vfxActions = vfx;
        }

        public override void Init()
        {
            parent.events.OnBounce += DoVFX;
        }

        public override void Teardown()
        {
            parent.events.OnBounce -= DoVFX;
        }

        private void DoVFX(Vector2 bouncePoint)
        {
            foreach (DeepVFXAction a in vfxActions)
            {
                a.Execute(bouncePoint);
            }
        }
    }
}
