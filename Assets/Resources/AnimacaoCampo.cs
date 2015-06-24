using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimacaoCampo : MonoBehaviour {
		
		void Start () {
			StartCoroutine(FramesCampo());
		}
		IEnumerator FramesCampo() {
			for (int i = 0; i < 18; i++) {
				gameObject.GetComponent<Image> ().sprite = Resources.Load<Sprite>("Texture/Campo_Texture_Sequence/campo_" + i);
				yield return new WaitForSeconds (0.04f);
			}
		}
		
}