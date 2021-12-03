using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.GameData
{
    public class DataTableManager : Singleton<DataTableManager>
    {
        public static ConstConfig ConstConfig { get; private set; }
        public static ItemRecordTable ItemTable { get; private set; }
        public static ItemTypeRecordTable ItemTypeTable { get; private set; }
        public static ItemMatchRecordTable ItemMatchTable { get; private set; }
        public static TextRecordTable Texts { get; private set; }
        public static ImageResourceRecordTable ImageTable { get; private set; }

        public static ColorRecordTable ColorTable { get; private set; }
        public static void Init()
        {
            ItemTable = new ItemRecordTable();
            ItemTypeTable = new ItemTypeRecordTable();
            ItemMatchTable = new ItemMatchRecordTable();
            Texts = new TextRecordTable();
            ImageTable = new ImageResourceRecordTable();
            ColorTable = new ColorRecordTable();
        }

        public static void Load()
        {
            ConstConfig.Load((result)=> { ConstConfig = result; });
            ItemTable.LoadJson();
            ItemTypeTable.LoadJson();
            ItemMatchTable.LoadJson();
            Texts.LoadJson();
            ImageTable.LoadJson();
            ColorTable.LoadJson();
        }

        public static bool IsCompleteLoad()
        {
            if (ConstConfig == null) return false;
            if (ItemTable == null) return false;
            if (ItemTypeTable == null) return false;
            if (ItemMatchTable == null) return false;
            if (Texts == null) return false;
            if (ImageTable == null) return false;
            if (ColorTable == null) return false;
            return true;
        }
    }
}