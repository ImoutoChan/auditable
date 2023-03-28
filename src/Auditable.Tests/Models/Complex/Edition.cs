using System;

namespace Auditable.Tests.Models.Complex
{
    public class Edition
    {
        protected int _hash;

        protected Edition()
        {
        }

        public Edition(string name, EditionType editionType)
        {
            Name = name;
            Type = editionType;
            _hash = Name.GetHashCode() + Type.GetHashCode();
        }

        public virtual DateTime ReleaseDate { get; set; }
        public virtual string Name { get; }
        public virtual EditionType Type { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Edition;
            if (other == null) return false;

            return other.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return _hash;
        }
    }
}