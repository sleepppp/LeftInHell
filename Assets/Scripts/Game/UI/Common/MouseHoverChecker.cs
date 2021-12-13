using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Project.UI
{
    public class MouseHoverChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Color ChangeColor;
        public Color OriginColor;
        public Image TargetImage;
        public void OnPointerEnter(PointerEventData eventData)
        {
            //todo caching
            if (Game.UIManager.GetUI<DragAndDropSystem>(UIKey.DragAndDropSystem).IsDragState)
                return;
            TargetImage.color = ChangeColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //todo caching
            if (Game.UIManager.GetUI<DragAndDropSystem>(UIKey.DragAndDropSystem).IsDragState)
                return;
            TargetImage.color = OriginColor;
        }
    }
}