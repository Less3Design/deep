using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deep.Views;

namespace Deep
{
    /// <summary>
    /// Extension methods and utilities for DeepEntity
    /// </summary>
    public static class DeepEntityUtility
    {
        //-----------------------------------
        //            RESOURCES
        //-----------------------------------

        public static bool AddResource(this DeepEntity e, D_Resource type, DeepResource resource)
        {
            e.resources.Add(type, resource);
            return true;
        }

        public static bool AddResource(this DeepEntity e, D_Resource type, R resourceTemplate)
        {
            if (e.resources.ContainsKey(type))
            {
                return false;
            }
            e.resources.Add(type, new DeepResource(resourceTemplate.baseMax, resourceTemplate.baseValue));
            return true;
        }

        //-----------------------------------
        //            ATTRIBUTES
        //-----------------------------------

        public static bool AddAttribute(this DeepEntity e, D_Attribute type, DeepAttribute attribute)
        {
            if (e.attributes.ContainsKey(type))
            {
                return false;
            }
            e.attributes.Add(type, attribute);
            return true;
        }

        public static bool AddAttribute(this DeepEntity e, D_Attribute type, A attributeTemplate)
        {
            if (e.attributes.ContainsKey(type))
            {
                return false;
            }

            DeepAttribute attribute;

            if (attributeTemplate.clamp)
            {
                attribute = new DeepAttribute(attributeTemplate.baseValue, attributeTemplate.minMax);
            }
            else
            {
                attribute = new DeepAttribute(attributeTemplate.baseValue);
            }

            e.attributes.Add(type, attribute);
            return true;
        }

        //-----------------------------------
        //            BEHAVIORS
        //-----------------------------------

        public static bool HasBehavior(this DeepEntity e, Type behavior)
        {
            foreach (DeepBehavior b in e.behaviors)
            {
                if (b.GetType() == behavior)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool TryToCast(this DeepEntity e, int index)
        {
            if (e.abilities.Count > index)
            {
                return e.abilities[index].Trigger();
            }
            Debug.LogWarning("Tried to cast missing index on: " + e.name + " Index: " + index);
            return false;
        }

        //-----------------------------------
        //            VIEWS
        //-----------------------------------

        public static DeepViewLink AddView(this DeepEntity entity, string view)
        {
            if (!DeepViewManager.instance.viewPool.ContainsKey(view) && !DeepViewManager.instance.RegisterView(view))
            {
                Debug.LogError("Failed to add view to entity");
                return null;
            }

            if (DeepViewManager.instance.viewPool[view].Count < 1)
            {
                DeepViewManager.instance.RegisterView(view, 1);
            }

            DeepViewLink v = DeepViewManager.PullView(view);
            v.transform.parent = entity.transform;
            v.transform.localPosition = Vector3.zero;
            v.transform.localRotation = Quaternion.identity;
            v.transform.localScale = Vector3.one;
            v.gameObject.SetActive(true);
            v.Setup(entity, view);
            entity.RefreshColliderSize();
            return v;
        }
    }
}
