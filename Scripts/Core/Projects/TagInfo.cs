using System;

namespace Camus.Projects
{
    public struct TagInfo : IEquatable<TagInfo>
    {
        public int Index
        {
            get;
        }

        public string Name
        {
            get;
        }

        public TagInfo(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public bool Equals(TagInfo other)
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

            TagInfo other = (TagInfo) obj;
            return this.Index == other.Index && this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Index ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"[Tag] #{Index} {Name}";
        }
    }
}
