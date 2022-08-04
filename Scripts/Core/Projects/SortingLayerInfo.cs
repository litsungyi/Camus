using System;

namespace Camus.Projects
{
    public struct SortingLayerInfo : IEquatable<SortingLayerInfo>
    {
        public int Index
        {
            get;
        }

        public string Name
        {
            get;
        }

        public SortingLayerInfo(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public bool Equals(SortingLayerInfo other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            SortingLayerInfo other = (SortingLayerInfo) obj;
            return this.Index == other.Index && this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Index ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"[SortingLayer] #{Index} {Name}";
        }
    }
}
