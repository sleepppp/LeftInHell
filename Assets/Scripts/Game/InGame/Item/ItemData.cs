
namespace Project
{
    using Project.GameData;
    public static class ItemData
    {
        public static ItemMatchRecord GetMatchRecord(int itemID, ItemMatchType matchType)
        {
            foreach(var record in DataTableManager.ItemMatchTable.Table)
            {
                if(record.Value.ItemID == itemID && 
                    record.Value.MatchType == (int)matchType)
                {
                    return record.Value;
                }
            }

            return null;
        }
    }
}