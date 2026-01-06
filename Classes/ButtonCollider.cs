using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static Fate.Menu.Main;
using static Fate.Settings;

namespace Fate.Classes
{
	public class Button : MonoBehaviour
	{
        public string relatedText;

		public static float buttonCooldown = 0f;
		
		public void OnTriggerEnter(Collider collider)
		{
			if (Time.time > buttonCooldown && collider == buttonCollider && menu != null)
			{
                buttonCooldown = Time.time + 0.2f;
				PlayButtonSound();
                Toggle(this.relatedText);
            }
		}
	}
}
