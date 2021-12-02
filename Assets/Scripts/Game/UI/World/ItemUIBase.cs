using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    using Project.GameData;
    public class ItemUIBase : MonoBehaviour
    {
        public Image ItemImage;
        public Text ItemNameText;
        public Text ItemDescriptionText;
        public Text ItemCountText;

        public void Init(Item item, int amount)
        {
            string path = DataTableManager.ImageTable.GetRecord(item.ItemRecord.ImageID).Path;
            AssetManager.LoadAssetAsync<Sprite>(path, (sprite) =>
            {
                SetItemImage(sprite);
            });
            SetItemCount(amount);
            SetItemName(item.Name);
        }

        void SetItemImage(Sprite sprite)
        {
            ItemImage.sprite = sprite;
        }

        void SetItemCount(int count)
        {
            ItemCountText.text = count.ToString();
        }

        void SetItemName(string name)
        {
            ItemNameText.text = name;
        }
    }
}