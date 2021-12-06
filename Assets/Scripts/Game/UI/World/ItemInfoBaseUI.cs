using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    using Project.GameData;
    public class ItemInfoBaseUI : MonoBehaviour
    {
        [SerializeField] Image m_itemImage;
        [SerializeField] Text m_itemNameText;
        [SerializeField] Text m_itemCountText;
        [SerializeField] Image m_itemBackgroundImage;

        public Color BackgroundColor { get { return m_itemBackgroundImage.color; } }

        public void Init(IGetItemData itemData)
        {
            Init(itemData.GetItemRecord().ID, itemData.Amount);
        }

        public void Init(int itemID, int itemCount)
        {
            ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemID);
            ItemTypeRecord typeRecord = DataTableManager.ItemTypeTable.GetRecord(itemRecord.Type);
            SetItemName(DataTableManager.Texts.GetRecord(itemRecord.DescID).Text);
            SetItemCount(itemCount);
            SetBackgroundColor(DataTableManager.ColorTable.GetRecord(typeRecord.ColorID).GetColor());
            string spritePath = DataTableManager.ImageTable.GetRecord(itemRecord.ImageID).Path;
            AssetManager.LoadAssetAsync<Sprite>(spritePath, (sprite) => 
            {
                SetItemSprite(sprite);
            });
        }

        public void SetBackgroundColor(Color color)
        {
            if (m_itemBackgroundImage != null)
            {
                m_itemBackgroundImage.color = color;
            }
        }

        void SetItemSprite(Sprite sprite)
        {
            if(m_itemImage != null)
            {
                m_itemImage.sprite = sprite;
            }
        }

        void SetItemName(string name)
        {
            if(m_itemNameText != null)
            {
                m_itemNameText.text = name;
            }
        }

        void SetItemCount(int count)
        {
            if(m_itemCountText != null)
            {
                m_itemCountText.text = count.ToString();
            }
        }
    }
}
