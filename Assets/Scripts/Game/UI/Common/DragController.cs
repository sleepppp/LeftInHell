using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Project.UI
{
    public class DragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] RectTransform m_controllTransform;

        Vector2 m_offset;

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_offset = eventData.position - (Vector2)m_controllTransform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_controllTransform.position = eventData.position - m_offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
}
