using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public static class WorldItemEntityTemplate
    {
        public static EntityTemplate Template(ItemDefinition item, int quantity = 1)
        {
            EntityTemplate t = EntityTemplate.Base();

            t.resources.Add(D_Resource.Health, new R(100));

            t.attributes.Add(D_Attribute.MoveSpeed, new A(10f));
            t.attributes.Add(D_Attribute.Drag, new A(2f));

            t.entityName = $"{item.itemName} x{quantity}";
            t.entityDescription = item.Description(quantity);

            //A world item is basically just a entity with the item in its inventory.
            t.inventoryVolume = 500M;
            t.initialInventory.Add(new ItemInInventory(item.className, quantity));

            t.behaviors.Add(new RandomStartVelocity());
            t.behaviors.Add(new AvoidOtherEntities(D_TeamSelector.Neutral, D_EntityTypeSelector.Item, 3f));

            t.team = D_Team.Neutral;
            t.type = D_EntityType.Item;

            return t;
        }
    }
}
