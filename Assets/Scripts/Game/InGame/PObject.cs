using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public abstract class PObject
    {
        protected readonly Puid m_puid;
        public virtual Puid Puid { get { return m_puid; } }

        public PObject()
        {
            m_puid = new Puid();
        }

        public PObject(PObject clone)
        {
            m_puid = new Puid(clone.Puid);
        }
    }
}
