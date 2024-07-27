using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public struct EntityTemplate
    {
        public Dictionary<D_Resource, R> resources;
        public Dictionary<D_Attribute, A> attributes;
        public List<DeepBehavior> behaviors;
        public D_Team team;
        public D_EntityType type;
        //Defining views in the template is optional. Sometimes its easier to define here, sometimes easier when creating entities.
        public string[] views;

        public EntityTemplate(Dictionary<D_Resource, R> resources, Dictionary<D_Attribute, A> attributes, List<DeepBehavior> behaviors, D_Team team, D_EntityType type, params string[] extraViews)
        {
            this.resources = resources;
            this.attributes = attributes;
            this.behaviors = behaviors;
            this.team = team;
            this.type = type;
            this.views = extraViews;
        }

        /// <summary>
        /// Creates a basic template with a HealthObserver behavior
        /// </summary>
        public static EntityTemplate Base()
        {
            EntityTemplate template = new EntityTemplate();
            template.resources = new Dictionary<D_Resource, R>();
            template.attributes = new Dictionary<D_Attribute, A>();
            template.behaviors = new List<DeepBehavior> { new HealthObserver() };
            return template;
        }
    }

    //resource template
    public struct R
    {
        public int baseMax { get; private set; }
        public int baseValue { get; private set; }

        public R(int baseMax)
        {
            this.baseMax = baseMax;
            this.baseValue = baseMax;
        }
        public R(int baseMax, int startValue)
        {
            this.baseMax = baseMax;
            this.baseValue = startValue;
        }
    }

    //attribute template
    public struct A
    {
        public float baseValue { get; private set; }
        public bool clamp { get; private set; }
        public Vector2 minMax { get; private set; }

        public A(float baseValue)
        {
            this.baseValue = baseValue;
            this.clamp = false;
            this.minMax = Vector2.zero;
        }
        public A(float baseValue, Vector2 minMax)
        {
            this.baseValue = baseValue;
            this.clamp = true;
            this.minMax = minMax;
        }
    }
}
