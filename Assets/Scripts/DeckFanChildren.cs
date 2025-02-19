using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckFanChildren : MonoBehaviour {
    void Start() {
		StartCoroutine(RotateChildren());
    }

	IEnumerator RotateChildren() {
		// wait for canvas to update and layout elements
		Canvas.ForceUpdateCanvases();
		yield return new WaitForEndOfFrame();
		Canvas.ForceUpdateCanvases();
		
		RectTransform parentRect = GetComponent<RectTransform>();
		float maxDist = transform.GetChild(0).gameObject.GetComponent<RectTransform>().position.x - parentRect.position.x;
		foreach (Transform tr in transform) {
			RectTransform rectChild = tr.gameObject.GetComponent<RectTransform>();
			float dist = rectChild.position.x - parentRect.position.x;
			float rotation = /*5.0f +*/ (dist / maxDist) * 5.0f; // modified for displaying 4+ cards
			rectChild.transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation);
		}
	}
}

