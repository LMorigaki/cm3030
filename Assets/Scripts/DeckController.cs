﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    GameObject cardContainer;
    UnityEngine.UI.Selectable selectable;

    private void Start()
    {
        cardContainer = transform.Find("CardsContainer").gameObject;
        selectable = GetComponent<UnityEngine.UI.Selectable>();
    }

    public void ShowDeck()
    {
        selectable.interactable = true;
    }

    public void HideDeck()
    {
        selectable.interactable = false;
    }

    // insert random cards

    // remove cards

    /// <summary>
    /// update layout of cards<br/>
    /// called when cards are inserted or removed
    /// </summary>
    public void FanCards()
    {
        StartCoroutine(UpdatePlaceHolders());
    }

    IEnumerator UpdatePlaceHolders()
    {
        yield return new WaitForFixedUpdate();
        cardContainer.GetComponent<DeckFanChildren>().FanCards();
        foreach (var e in cardContainer.GetComponentsInChildren<UIBringToFront>())
        {
            e.UpdateIndex();
        }
    }
}
