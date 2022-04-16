using System;

namespace Camus.Projects
{
    public struct SceneInfo : IEquatable<SceneInfo>
    {
        public int Index
        {
            get;
        }

        public string Name
        {
            get;
        }

        public SceneInfo(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public bool Equals(SceneInfo other)
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

            SceneInfo other = (SceneInfo) obj;
            return this.Index == other.Index && this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Index ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"[Scene] #{Index} {Name}";
        }
    }
}
