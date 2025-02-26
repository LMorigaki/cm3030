using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBringToFront : MonoBehaviour {
	public GameObject target;
	private int originalPos;
	
	private GameObject empty;
	Transform folder;


	void Start() {
        originalPos = target.transform.GetSiblingIndex();
		empty = new GameObject();
		
		// add rect to ghost object
		RectTransform rect = empty.AddComponent(typeof(RectTransform)) as RectTransform;
		RectTransform targetRect = target.GetComponent<RectTransform>();
		rect.anchorMin = targetRect.anchorMin;
		rect.anchorMax = targetRect.anchorMax;
		rect.anchoredPosition = targetRect.anchoredPosition;
		rect.sizeDelta = targetRect.sizeDelta;
		
		// disable the ghost object
		empty.SetActive(false);
		folder = GameObject.FindGameObjectWithTag("DeckFolder").transform.Find("Placeholders");
		empty.transform.SetParent(folder);
	}

	void BringToFront() {
		target.GetComponent<LayoutElement>().ignoreLayout = true;
		target.transform.SetAsLastSibling();

		// replace with empty object
		empty.SetActive(true);
		empty.transform.SetParent(target.transform.parent);
		empty.transform.SetSiblingIndex(originalPos);
	}

	void RestorePosition() {
		target.GetComponent<LayoutElement>().ignoreLayout = false;
		target.transform.SetSiblingIndex(originalPos);

		empty.SetActive(false);
		empty.transform.SetParent(folder.transform);
	}

	public void UpdateIndex()
    {
		originalPos = target.transform.GetSiblingIndex();
		RectTransform rect = empty.GetComponent<RectTransform>();
		RectTransform targetRect = target.GetComponent<RectTransform>();
		rect.anchorMin = targetRect.anchorMin;
		rect.anchorMax = targetRect.anchorMax;
		rect.anchoredPosition = targetRect.anchoredPosition;
		rect.sizeDelta = targetRect.sizeDelta;
	}

    private void OnDestroy()
    {
		Destroy(empty);
    }
}
