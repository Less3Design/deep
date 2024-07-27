using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class DieAfterTime : DeepBehavior
    {
        public float time;
        private bool waiting;
        private Coroutine coroutine;

        public DieAfterTime(float time)
        {
            this.time = time;
        }

        public override void Init()
        {
            waiting = false;
            coroutine = parent.StartCoroutine(WaitCo());
        }

        public override void Teardown()
        {
            if (waiting)
            {
                parent.StopCoroutine(coroutine);
            }
        }

        private IEnumerator WaitCo()
        {
            yield return new WaitForSeconds(time);
            waiting = false;
            new KillAction(parent).Execute();
        }
    }
}