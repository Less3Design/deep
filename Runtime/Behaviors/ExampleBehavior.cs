using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Deep
{
    public class ExampleBehavior : DeepBehavior
    {
        public DeepAttributeModifier strMod = new DeepAttributeModifier(10f,0f,0f);

        public override void Init()
        {
            parent.attributes[D_Attribute.Strength].AddModifier(strMod);
            parent.StartCoroutine(DestroyCo(parent));
        }

        IEnumerator DestroyCo(DeepEntity deepEntity)
        {
            yield return new WaitForSeconds(5f);
            new RemoveBehaviorAction(parent,owner,this).Execute();
        }
        
        public override void Teardown()
        {
            parent.attributes[D_Attribute.Strength].RemoveModifer(strMod);
        }
    }
}