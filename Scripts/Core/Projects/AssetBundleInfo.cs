using System;

namespace Camus.Projects
{
    public struct AssetBundleInfo : IEquatable<AssetBundleInfo>
    {
        public string Name
        {
            get;
        }

        public AssetBundleInfo(string name)
        {
            Name = name;
        }

        public bool Equals(AssetBundleInfo other)
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

            AssetBundleInfo other = (AssetBundleInfo) obj;
            return this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"[AssetBundle] #{Name}";
        }
    }
}
