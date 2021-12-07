using System;

namespace Project
{

    [Serializable]
    public class Puid : IClone<Puid>
    {
        [NonSerialized] Guid _guid;

        public Puid()
        {
            _guid = Guid.NewGuid();
        }

        public Puid(Puid muid)
        {
            _guid = muid._guid;
        }

        public bool IsValid()
        {
            return _guid == Guid.Empty;
        }

        public static bool operator ==(Puid left, Puid right)
        {
            return object.ReferenceEquals(left, right);
        }

        public static bool operator !=(Puid left, Puid right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Puid);
        }
        public bool Equals(Puid obj)
        {
            return object.ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }

        public Puid Clone()
        {
            return new Puid(this);
        }
    }
}