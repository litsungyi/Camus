using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace Camus.Localization
{
    public class LocalStringUpdater : MonoBehaviour
    {
		[SerializeField] private Text Text;
		[SerializeField] private string Key = string.Empty;

		private LocalKey localKey;

        private void Start()
        {
            if ( Text == null || string.IsNullOrEmpty( Key ) )
            {
                return;
            }

            localKey = LocalKey.Create( Key );
            Text.text = localKey.Value;
        }

        private void FixedUpdate()
        {
            if ( Text == null || string.IsNullOrEmpty( Key ) )
            {
                return;
            }

            Text.text = localKey.Value;
        }
    }
}
