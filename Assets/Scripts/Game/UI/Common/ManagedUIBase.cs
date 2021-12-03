using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class ManagedUIBase : UIBase
    {
        [Header("ManagedUIBase")]
        [SerializeField] protected UIKey _uiKey;

        public UIKey UIKey { get { return _uiKey; } set { _uiKey = value; } }

        public virtual void Close()
        {
            Game.UIManager.RemoveUI(_uiKey);
        }
    }
}
