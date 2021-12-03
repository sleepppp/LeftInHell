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
            AsyncSetItemImage(path);
            SetItemCount(amount);
            SetItemName(item.Name);
        }

        void AsyncSetItemImage(string path)
        {
            //todo SetLoadImage
            AssetManager.LoadAssetAsync<Sprite>(path, (sprite) =>
            {
                SetItemImage(sprite);
            });
        }

        void SetItemImage(Sprite sprite)
        {
            if (ItemImage != null)
            {
                ItemImage.sprite = sprite;
            }
        }

        void SetItemCount(int count)
        {
            if (ItemCountText != null)
            {
                ItemCountText.text = count.ToString();
            }
        }

        void SetItemName(string name)
        {
            if (ItemNameText)
            {
                ItemNameText.text = name;
            }
        }
    }
}