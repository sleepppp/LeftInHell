using System.Collections.Generic;
using UnityEngine;
namespace Project.GameData
{
    public static class ItemDataTable
    {
        public static Vector2Int GetItemContainerSize(int itemRecordID)
        {
            Vector2Int result = Vector2Int.zero;
            ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemRecordID);
            if (itemRecord == null)
                return result;
            Dictionary<int, ItemMatchRecord> records = DataTableManager.ItemMatchTable.Table;
            foreach(var record in records)
            {
                if(record.Value.ItemID == itemRecordID)
                {
                    if (record.Value.MatchType == (int)ItemMatchType.ItemContainerWidth)
                        result.x = record.Value.Value;
                    else if (record.Value.MatchType == (int)ItemMatchType.ItemContainerHeight)
                        result.y = record.Value.Value;

                    if(result.x != 0 && result.y != 0)
                    {
                        break;
                    }
                }
            }

            return result;
        }
    }
}
