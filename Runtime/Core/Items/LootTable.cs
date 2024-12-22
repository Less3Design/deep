using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class LootTable
    {
        public List<L> table;

        public LootTable(params L[] items)
        {
            table = new List<L>();
            foreach (L l in items)
            {
                table.Add(l);
            }
        }

        public void RollTable()
        {
            int totalWeight = 0;
            foreach (L l in table)
            {
                totalWeight += l.weight;
            }

            int roll = Random.Range(0, totalWeight);
            int currentWeight = 0;
            foreach (L l in table)
            {
                currentWeight += l.weight;
                if (roll < currentWeight)
                {
                    Debug.LogError("NOT IMPLEMENTED");
                    return;
                }
            }
        }
    }

    public struct L
    {
        public ItemDefinition item;
        public int weight;
        public int level;

        public L(int weight, ItemDefinition item, int level)
        {
            this.item = item;
            this.weight = weight;
            this.level = level;
        }
    }

    public static class LootTables
    {
    }
}
