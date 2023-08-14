using System;

namespace Camus.Projects
{
    public struct LayerInfo : IEquatable<LayerInfo>
    {
        public int Index
        {
            get;
        }

        public string Name
        {
            get;
        }

        public LayerInfo(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public bool Equals(LayerInfo other)
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

            LayerInfo other = (LayerInfo) obj;
            return this.Index == other.Index && this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Index ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"[Layer] #{Index} {Name}";
        }
    }
}
