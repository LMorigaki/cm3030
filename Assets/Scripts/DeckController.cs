﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public readonly int maxCardCount = 6;
    GameObject card;
    GameObject cardContainer;
    UnityEngine.UI.Selectable selectable;

    private void Awake()
    {
        card = Resources.Load<GameObject>("Prefabs/CardButton");
        cardContainer = transform.Find("CardsContainer").gameObject;
        selectable = GetComponent<UnityEngine.UI.Selectable>();
    }

    private void Start()
    {
        
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
    public void InsertRandomCards(int? amount = null)
    {
        if (amount == null)
        {
            amount = maxCardCount - cardContainer.transform.childCount;
        }
        for (int i = 0; i < (int)amount; i++)
        {
            BuildingCard _card = BuildingCard.RandomBuildingCard();
            GameObject newcard = GameObject.Instantiate(card, cardContainer.transform);
            newcard.GetComponent<CardBehaviour>().SetCardInfo(_card);
            newcard.GetComponent<CardBehaviour>().Activate();
        }
    }

    /// <summary>
    /// Moves a card object from shop to card deck
    /// </summary>
    public void InsertFromShop(GameObject cardObject)
    {
        cardObject.transform.parent = cardContainer.transform;
        cardObject.GetComponent<CardBehaviour>().Activate();
        FanCards();
    }

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
