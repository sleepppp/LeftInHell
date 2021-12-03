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
	public class ColorRecord 
	{
		public int ID;
		public string DEV;
		public int R;
		public int G;
		public int B;
		public int A;
	}
	public class ColorRecordList 
	{
		public List<ColorRecord> List;
	}
	public class ColorRecordTable 
	{
		public Dictionary<int,ColorRecord> Table;
		public void LoadJson()
		{
			string filePath = "Assets/Data/Json/Color.json";
			AssetManager.LoadAssetAsync<TextAsset>(filePath, (loadResult) =>
			{
				Table = new Dictionary<int,ColorRecord>();
				ColorRecordList list = JsonConvert.DeserializeObject<ColorRecordList>(loadResult.text);
				for (int i = 0; i < list.List.Count; ++i)
				{
					Table.Add(list.List[i].ID, list.List[i]);
				}
			}
			);
		}
		public ColorRecord GetRecord(int id)
		{
			ColorRecord result = null;
			Table.TryGetValue(id, out result);
			return result;
		}
	}
}
