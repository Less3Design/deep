using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public abstract class ItemDefinition
    {
        public string className => this.GetType().Name;
        public virtual string itemName { get; } = "Unnamed Item";
        public virtual bool isStackable { get; } = false;
        public virtual int maxStackQuantity { get; } = 8;
        public virtual int itemValue { get; } = 1;
        public virtual decimal itemVolume { get; } = 0.001M;
        public virtual Color itemColor { get; } = Color.white;

        public virtual string sprite { get; } = "";
        /// <summary>Views added to an entity when placed in entity inventory</summary>
        public virtual string[] inEntityInventoryViews { get; } = default;

        public virtual List<DeepBehavior> ItemBehaviors(int quantity) { return new List<DeepBehavior>(); }
        public virtual string Description(int quantity) { return "Missing Description"; }
        public virtual string AttributeDescription(int quantity) { return "Missing Attribute Description"; }
        public virtual string InInventoryName(int quantity)
        {
            if (isStackable)
            {
                return $"<color=#{ColorUtility.ToHtmlStringRGBA(itemColor)}>{itemName} x{quantity}</color>";
            }
            else
            {
                return $"<color=#{ColorUtility.ToHtmlStringRGBA(itemColor)}>{itemName}</color>";
            }
        }
    }

    public class ItemInInventory
    {
        public string itemClassName;
        public int quantity;
        [HideInInspector]
        public ItemDefinition itemDefinition => Items.GetItem(itemClassName);

        public ItemInInventory(string itemClassName, int quantity = 1)
        {
            this.itemClassName = itemClassName;
            this.quantity = quantity;
        }
    }

    /// <summary>
    /// The items class contains a static instance of every item in the game.
    /// This is a widely spread partial class. Items should be defined inside the class next to their ItemDefinition's
    /// </summary>
    /// Does this seem weird? We are effectively acheiving an abstract static class with this.
    public static partial class Items
    {
        /// <summary>
        /// A lookup of all items included in the class populated at runtime with reflection. Key is classname.
        /// </summary>
        public static Dictionary<string, ItemDefinition> itemLookup { get; private set; }

        static Items()
        {
            itemLookup = new Dictionary<string, ItemDefinition>();
            foreach (Type type in Assembly.GetAssembly(typeof(ItemDefinition)).GetTypes())
            {
                if (type.IsClass && type.IsSubclassOf(typeof(ItemDefinition)) && !type.IsAbstract)
                {
                    ItemDefinition item = (ItemDefinition)Activator.CreateInstance(type);
                    itemLookup.Add(item.className, item);
                }
            }
        }

        public static bool TryGetItem(string className, out ItemDefinition item)
        {
            return itemLookup.TryGetValue(className, out item);
        }

        public static ItemDefinition GetItem(string className)
        {
            if (itemLookup.TryGetValue(className, out ItemDefinition item))
            {
                return item;
            }
            return null;
        }
    }

    public static partial class Items
    {
        public static ItemDefinition ExampleItem = new ExampleItem();
    }

    public class ExampleItem : ItemDefinition
    {
        public override string itemName { get; } = "Example Item";
        public override string sprite => "ItemArmor";

        public override List<DeepBehavior> ItemBehaviors(int quantity)
        {
            return new List<DeepBehavior>()
            {
                new StaticAttributeMod(
                    D_Attribute.MoveSpeed,
                    new DeepAttributeModifier(multiplier: .07f * quantity)
                )
            };
        }
    }
}
