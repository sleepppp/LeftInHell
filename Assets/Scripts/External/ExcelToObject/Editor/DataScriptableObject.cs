
using UnityEngine;
using UnityEditor;

namespace Project.Editor
{
    public class DataScriptableObject : ScriptableObject
    {
        [MenuItem("Project/Data/Create/CreateLoadInfoAsset")]
        static void Create()
        {
            DataScriptableObject instance = CreateInstance<DataScriptableObject>();

            if (AssetDatabase.IsValidFolder("Assets/Data") == false)
            {
                AssetDatabase.CreateFolder("Assets", "Data");
            }

            AssetDatabase.CreateAsset(instance, "Assets/Data/LoadData.asset");
        }

        [Header("CodeGenerateOption")]
        public string NameSpace;

        [Header("Folder")]
        public UnityEngine.Object ExcelFolder;
        public UnityEngine.Object CodeFolder;
        public UnityEngine.Object JsonFolder;
    }
}