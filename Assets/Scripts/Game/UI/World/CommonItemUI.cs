using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    using Project.GameData;
    public class CommonItemUI : MonoBehaviour
    {
        [SerializeField] Image m_itemImage;
        [SerializeField] Image m_backgroundImage;
        [SerializeField] Text m_itemNameText;
        [SerializeField] Text m_itemAmountText;

        Color m_originColor;

        public float OriginBackgroundAlpha { get { return m_originColor.a; } }
        public Color OriginColor { get { return m_originColor; } }

        public void Init(int itemID, int amount)
        {
            m_originColor = m_backgroundImage.color;
            ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemID);
            ItemTypeRecord typeRecord = DataTableManager.ItemTypeTable.GetRecord(itemRecord.Type);

            SetNameText(DataTableManager.Texts.GetRecord(itemRecord.DescID).Text);
            SetAmount(amount);
            Color color = DataTableManager.ColorTable.GetRecord(typeRecord.ColorID).GetColor();
            SetBackgroundColor(new Color(color.r, color.g, color.b, OriginBackgroundAlpha));
            string path = DataTableManager.ImageTable.GetRecord(itemRecord.ImageID).Path;
            AssetManager.LoadAssetAsync<Sprite>(path, (sprite) => { SetItemSprite(sprite); });
        }

        void SetItemSprite(Sprite sprite)
        {
            if(m_itemImage != null)
            {
                m_itemImage.sprite = sprite;
            }
        }

        void SetBackgroundColor(Color color)
        {
            if(m_backgroundImage != null)
            {
                m_backgroundImage.color = color;
            }
        }

        void SetNameText(string name)
        {
            if(m_itemNameText != null)
            {
                m_itemNameText.text = name;
            }
        }

        void SetAmount(int amount)
        {
            if(m_itemAmountText != null)
            {
                m_itemAmountText.text = amount.ToString();
            }
        }
    }
}