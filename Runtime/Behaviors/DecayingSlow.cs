using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Deep
{
    public class DecayingSlow : DeepBehavior
    {
        public float modValue = -.5f;
        DeepAttributeModifier speedMod;
        public float duration = 5f;

        private DeepViewLink viewRef;

        public override void Init()
        {
            speedMod = new DeepAttributeModifier(0f, modValue, 0f);
            parent.attributes[D_Attribute.MoveSpeed].AddModifier(speedMod);
            parent.StartCoroutine(Decay());
            viewRef = parent.AddView("DecayingSlowView");
        }

        private float timer;
        public IEnumerator Decay()
        {
            while (timer < duration)
            {
                speedMod.UpdateModifier(0f, Mathf.Lerp(modValue, 0f, timer / duration), 0f);
                timer += Time.deltaTime;
                yield return null;
            }
            new RemoveBehaviorAction(parent,owner,this).Execute();
        }

        public override void Teardown()
        {
            parent.attributes[D_Attribute.MoveSpeed].RemoveModifer(speedMod);
            viewRef.StartReturn();
        }
    }
}