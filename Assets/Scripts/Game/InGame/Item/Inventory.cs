using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    using Project.GameData;
    public class Inventory
    {
        public ItemBag Bag { get;private set; }
        public ItemBag TestBag { get; private set; }

        public int CurrentWeight { get; private set; }

        public int MaxWeightCoverage { get; private set; }

        public Inventory() 
        {
            MaxWeightCoverage = DataTableManager.ConstConfig.DefaultWeightCoverage;
            CurrentWeight = 0;

            //test init
            Bag = new ItemBag(DataTableManager.ItemTable.GetRecord(3));
            if(Bag.TryAddItem(1, 100) == false)
            {
                Debug.LogError("?");
            }
            if(Bag.TryAddItem(4,13)== false)
            {
                Debug.LogError("??");
            }
            if(Bag.TryAddItem(5,40) == false)
            {
                Debug.LogError("???");
            }
            if(Bag.TryAddItem(1,30) == false)
            {
                Debug.LogError("????");
            }

            TestBag = new ItemBag(DataTableManager.ItemTable.GetRecord(3));
        }

        public bool CanAddItem(ItemRecord itemRecord, int amount)
        {
            bool isWeightOK =  CurrentWeight + itemRecord.Weight * amount <= MaxWeightCoverage;
            if (isWeightOK == false)
                return false;
            if (Bag.CanAddItem(itemRecord.ID, amount))
                return true;
            //todo check Equip
            return true;
        }

        public bool TryAddItem(ItemRecord itemRecord, int amount)
        {
            if (CanAddItem(itemRecord, amount) == false)
                return false;
            //todo check equip
            if (Bag.TryAddItem(itemRecord.ID, amount) == false)
                return false;

            Refresh();

            return true;
        }

        public bool TryRemoveItem(ItemBlock itemBlock, int amount)
        {
            if (Bag.TryRemoveItem(itemBlock, amount) == false)
                return false;

            //todo check equip
            return true;
        }

        public void Refresh()
        {
            CurrentWeight = Bag.GetTotalWeight();
        }
    }
}