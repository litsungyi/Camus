namespace Camus.Localization
{
    // Localization Key
    public class LocalKey
    {
        public LocalKey( string key )
        {
            Key = key;
        }

        public string Key
        {
            get;
            private set;
        }

        public string Value
        {
            get
            {
				return App.Instance.Localization.GetLocalString( this );
            }
        }

		public override string ToString()
		{
			return Key;
		}

		public override int GetHashCode()
		{
			return Key.GetHashCode ();
		}

		public override bool Equals ( object obj )
		{
			return Equals(obj as LocalKey);
		}
		
		public bool Equals ( LocalKey obj )
		{
			return obj != null && obj.GetHashCode () == this.GetHashCode ();
		}

		public static implicit operator string ( LocalKey localKey )
		{
			return App.Instance.Localization.GetLocalString( localKey );
		}
    }
}
