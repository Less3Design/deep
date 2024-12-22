using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public enum InventoryAddResponse
    {
        Success,
        Failed,
        PartiallyAdded,//Happens when adding a stackable item that can't fit the whole stack.
    }

    public class DeepEntityInventory
    {
        private const int MAX_ITEMS = 10;//TODO remove.

        /// <summary>
        /// items list acts like an array with null items filled in. Non-null items start at [0]
        /// </summary>
        public List<ItemInInventory> items { get; private set; }
        [HideInInspector]
        public int maxItems { get; private set; }
        [HideInInspector]
        public DeepEntity entity { get; private set; }
        private Dictionary<ItemInInventory, List<RemoveBehaviorAction>> itemBehaviors;

        /// <summary>
        /// Called anytime an inventory-changing method is called. Does not guaruntee that the inventory actually changed.
        /// </summary>
        public event Action OnInventoryUpdated;

        public DeepEntityInventory(DeepEntity entity)
        {
            this.entity = entity;
            this.maxItems = MAX_ITEMS;
            items = new List<ItemInInventory>();
            for (int i = 0; i < maxItems; i++)
            {
                items.Add(null);
            }
            itemBehaviors = new Dictionary<ItemInInventory, List<RemoveBehaviorAction>>();
        }

        /// <summary>
        /// Call after making a change directly to an ItemInInventory instance conatined in items.
        /// </summary>
        public void UpdateItem(ItemInInventory item)
        {
            if (items.Contains(item) && itemBehaviors.ContainsKey(item))
            {
                RemoveItemBehaviors(item);
                AddItemBehaviors(item);
                OnInventoryUpdated?.Invoke();
            }
        }

        /// <summary>
        /// Try to add the item to the inventory. If stackable this will fill in stacks first. 
        /// An item can be partially added if it is stackable and there is not enough space for the whole stack. In this case
        /// the passed item will have its quantity updated.
        /// </summary>
        public InventoryAddResponse TryAddItem(ItemInInventory item)
        {
            if (item == null || item.quantity <= 0 || maxItems <= 0)
            {
                return InventoryAddResponse.Failed;
            }

            //add quantity to existing stacks if relevant. This causes behaviors to refresh.
            bool addedToStack = false;
            if (item.itemDefinition.isStackable)
            {
                bool consumedByStacking = false;
                List<ItemInInventory> behaviorsToRefresh = new List<ItemInInventory>();
                for (int i = 0; i < items.Count; i++)
                {
                    ItemDefinition itemInInventoryDefinition = items[i] == null ? null : items[i].itemDefinition;
                    if (items[i] != null && items[i].itemClassName == item.itemClassName && items[i].quantity < itemInInventoryDefinition.maxStackQuantity)
                    {
                        int adding = Mathf.Min(itemInInventoryDefinition.maxStackQuantity - items[i].quantity, item.quantity);
                        items[i].quantity += adding;
                        item.quantity -= adding;
                        behaviorsToRefresh.Add(items[i]);
                        addedToStack = true;
                    }
                    if (item.quantity <= 0)
                    {
                        consumedByStacking = true;
                        break;
                    }
                }

                foreach (ItemInInventory itemToRefresh in behaviorsToRefresh)
                {
                    RemoveItemBehaviors(itemToRefresh);
                    AddItemBehaviors(itemToRefresh);
                }

                if (consumedByStacking)
                {

                    OnInventoryUpdated?.Invoke();
                    return InventoryAddResponse.Success;
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    AddItemBehaviors(item);
                    OnInventoryUpdated?.Invoke();
                    return InventoryAddResponse.Success;
                }
            }

            if (addedToStack)
            {
                //! An item was partially added. It filled up some stacks, but there was a remainder, and no space for it.
                OnInventoryUpdated?.Invoke();
                return InventoryAddResponse.PartiallyAdded;
            }
            return InventoryAddResponse.Failed;
        }

        public bool RemoveItemAtIndex(int index, out ItemInInventory removedItem, bool reorder = true)
        {
            if (index < 0 || index >= items.Count || items[index] == null)
            {
                removedItem = null;
                return false;
            }
            RemoveItemBehaviors(items[index]);
            removedItem = items[index];
            items.RemoveAt(index);
            if (reorder)
            {
                items.Add(null);
            }
            else
            {
                items.Insert(index, null);
            }
            OnInventoryUpdated?.Invoke();
            return true;
        }

        public bool DropItemIntoWorld(int index, bool reorder = true)
        {
            if (index < 0 || index >= items.Count || items[index] == null)
            {
                return false;
            }
            RemoveItemAtIndex(index, out ItemInInventory removedItem, reorder);
            DeepEntity.Create(WorldItemEntityTemplate.Template(removedItem.itemDefinition, removedItem.quantity), entity.cachedTransform.position, Quaternion.identity);
            return true;
        }

        public void SwapItems(int indexA, int indexB)
        {
            if (indexA < 0 || indexA >= items.Count || indexB < 0 || indexB >= items.Count)
            {
                return;
            }
            ItemInInventory temp = items[indexA];
            items[indexA] = items[indexB];
            items[indexB] = temp;
            OnInventoryUpdated?.Invoke();
        }

        //* BEHAVIORS

        private void AddItemBehaviors(ItemInInventory item)
        {
            if (itemBehaviors.ContainsKey(item))
            {
                throw new System.Exception("Adding item behaviors but entity already has behaviors for this item.");
            }
            List<DeepBehavior> behaviors = item.itemDefinition.ItemBehaviors(item.quantity);
            List<RemoveBehaviorAction> removeActions = new List<RemoveBehaviorAction>();
            if (behaviors != null)
            {
                foreach (DeepBehavior behavior in behaviors)
                {
                    var add = new AddBehaviorAction(entity, entity, behavior);
                    var removeAction = add.CreateRemoveAction(entity);
                    add.Execute();
                    removeActions.Add(removeAction);
                }
            }
            itemBehaviors.Add(item, removeActions);
        }

        private void RemoveItemBehaviors(ItemInInventory item)
        {
            if (!itemBehaviors.ContainsKey(item))
            {
                throw new System.Exception("Removing item behaviors but entity does not have behaviors for this item.");
            }
            foreach (RemoveBehaviorAction removeAction in itemBehaviors[item])
            {
                removeAction.Execute();
            }
            itemBehaviors.Remove(item);
        }
    }
}
