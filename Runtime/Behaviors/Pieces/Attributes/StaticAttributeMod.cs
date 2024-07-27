using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class StaticAttributeMod : DeepBehavior
    {
        public D_Attribute targetAttribute;
        public DeepAttributeModifier attributeModifier;

        private DeepAttributeModifier activeMod;

        public StaticAttributeMod(D_Attribute targetAttribute, DeepAttributeModifier attributeModifier)
        {
            this.targetAttribute = targetAttribute;
            this.attributeModifier = attributeModifier;
        }

        public override void Init()
        {
            activeMod = new DeepAttributeModifier(attributeModifier);
            activeMod.owner = owner;
            parent.attributes[targetAttribute].AddModifier(activeMod);
        }

        public override void Teardown()
        {
            parent.attributes[targetAttribute].RemoveModifer(activeMod);
        }
    }
}