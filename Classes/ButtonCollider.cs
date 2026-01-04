using UnityEngine;
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
                GorillaTagger.Instance.StartVibration(rightHanded, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 2f);
                VRRig.LocalRig.PlayHandTapLocal(66, rightHanded, 5f);
				Toggle(this.relatedText);
            }
		}
	}
}
