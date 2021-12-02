using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
////==============================================================================
////이 코드는 자동생성을 통해 생성되었습니다
////파일 내용을 수정하면 잘못된 동작이 발생할 수 있습니다
////만약 생성 오류가 발생했다면 지우고 다시 시도해 주시길 바랍니다
////==============================================================================
namespace Project.GameData
{
	public class ItemRecord 
	{
		public int ID;
		public string DEV;
		public int DescID;
		public int Type;
		public int MaxStackAmount;
		public int Width;
		public int Height;
		public int ModelID;
		public int ImageID;
		public int Weight;
	}
	public class ItemRecordList 
	{
		public List<ItemRecord> List;
	}
	public class ItemRecordTable 
	{
		public Dictionary<int,ItemRecord> Table;
		public void LoadJson()
		{
			string filePath = "Assets/Data/Json/Item.json";
			AssetManager.LoadAssetAsync<TextAsset>(filePath, (loadResult) =>
			{
				Table = new Dictionary<int,ItemRecord>();
				ItemRecordList list = JsonConvert.DeserializeObject<ItemRecordList>(loadResult.text);
				for (int i = 0; i < list.List.Count; ++i)
				{
					Table.Add(list.List[i].ID, list.List[i]);
				}
			}
			);
		}
		public ItemRecord GetRecord(int id)
		{
			ItemRecord result = null;
			Table.TryGetValue(id, out result);
			return result;
		}
	}
}
