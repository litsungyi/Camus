using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace Camus.Localization
{
	[Serializable]
    public class LocalStringUpdater : MonoBehaviour
    {
        public Text Text;

        public string Key = string.Empty;

        private void Awake()
        {
            if ( Text == null || string.IsNullOrEmpty( Key ) )
            {
                return;
            }

            var localKey = new LocalKey( Key );
            Text.text = localKey.Value;
        }

        private void FixedUpdate()
        {
            if ( Text == null || string.IsNullOrEmpty( Key ) )
            {
                return;
            }

            var localKey = new LocalKey( Key );
            Text.text = localKey.Value;
        }
    }
}
