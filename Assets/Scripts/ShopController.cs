using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public readonly int maxCardCount = 10;
    GameObject card;
    GameObject cardContainer;

    private void Awake()
    {
        card = Resources.Load<GameObject>("Prefabs/CardButton");
        cardContainer = transform.Find("CardsContainer").gameObject;
    }

    /// <summary>
    /// insert random building and event cards into shop
    /// </summary>
    /// <param name="amount">desinated amount of cards,<br/>
    /// no more cards will be inserted if shop is full<br/>
    /// if not specified, insert cards until shop is full</param>
    public void InsertRandomCards(int? amount = null)
    {
        if (amount == null || amount > maxCardCount - cardContainer.transform.childCount)
        {
            amount = maxCardCount - cardContainer.transform.childCount;
        }
        for (int i = 0; i < (int)amount; i++)
        {
            Card _card;
            if (Random.Range(0, 1f) > 0.5f)
            {
                _card = BuildingCard.RandomBuildingCard();
            }
            else
            {
                _card = EventCard.RandomEventCard();
            }
            GameObject newcard = GameObject.Instantiate(card, cardContainer.transform);
            newcard.GetComponent<CardBehaviour>().SetCardInfo(_card);
            newcard.GetComponent<CardBehaviour>().inShop = true;
        }
    }

    /// <summary>
    /// remove all cards in shop
    /// </summary>
    public void RemovelAll()
    {
        for (int i = 0; i < cardContainer.transform.childCount; i++)
        {
            Destroy(cardContainer.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// remove all then insert new cards to shop
    /// </summary>
    public void RerollAll()
    {
        RemovelAll();
        InsertRandomCards();
    }

}
