using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Stores card info and provides behaviour of a card object
/// </summary>
public class CardBehaviour : MonoBehaviour
{
    /// <summary>
    /// Card info of this card object
    /// </summary>
    public Card card;

    [HideInInspector]
    /// <summary>
    /// if this card is displaying in shop
    /// </summary>
    public bool inShop = false;

    public Animator animator;
    public UIBringToFront bringToFront;
    public Button button;
    public GameObject priceDisplayer;

    /// <summary>
    /// Initialize child objects base on given card info
    /// </summary>
    /// <param name="card"></param>
    public void SetCardInfo(Card card)
    {
        this.card = card;
        Transform _cardButton = transform.Find("CardButton");
        GameObject _title = _cardButton.Find("Title").gameObject;
        _title.GetComponent<TextMeshProUGUI>().text = card.title;

        GameObject _description = _cardButton.Find("Description").gameObject;
        _description.GetComponent<TextMeshProUGUI>().text = card.description;

        if (card.type == CardType.Building)
        {
            GameObject _image = _cardButton.Find("Image").gameObject;
            _image.GetComponent<Image>().sprite = ModelManager.LoadImage(((BuildingCard)card).buildingID);

            GameObject _income = _cardButton.Find("Income").gameObject;
            _income.GetComponent<TextMeshProUGUI>().text = "▲" + ((BuildingCard)card).profit;

            GameObject _upkeep = _cardButton.Find("Upkeep").gameObject;
            _upkeep.GetComponent<TextMeshProUGUI>().text = "▼" + ((BuildingCard)card).upkeep;
        }
        if (card.type == CardType.Event)
        {
            _cardButton.Find("Income").gameObject.SetActive(false);
            _cardButton.Find("Upkeep").gameObject.SetActive(false);
        }
        transform.Find("CardButton").GetComponent<Button>().interactable = true;
        if (inShop)
        {
            priceDisplayer.SetActive(true);
            priceDisplayer.GetComponent<TextMeshProUGUI>().text = "$" + card.price.ToString();
        }
    }

    /// <summary>
    /// OnClick is called when the card object attached with this script is clicked
    /// </summary>
    public void OnClick()
    {
        if (inShop)
        {
            if (GameObject.Find("EventSystem").GetComponent<GameFlowController>().BuyCard(card.price))
            {
                if (card.type == CardType.Building)
                {
                    DeckController deckController = GameObject.FindGameObjectWithTag("DeckFolder").GetComponent<DeckController>();
                    deckController.InsertFromShop(gameObject);
                    priceDisplayer.SetActive(false);
                    inShop = false;
                }
                else if (card.type == CardType.Event)
                {
                    TilemapController tilemapController = GameObject.FindGameObjectWithTag("TilemapController").GetComponent<TilemapController>();
                    tilemapController.board.ApplyEvent((EventCard)card);
                    Destroy(gameObject);
                }
            }
            return;
        }
        else
        {
            switch (card.type)
            {
                case CardType.Building:
                    // instantiate placeable object
                    GameObject placeable = Resources.Load<GameObject>("Prefabs/Placeable");
                    placeable.GetComponent<Placeable>().cardBehaviour = this;
                    Transform folder = GameObject.FindGameObjectWithTag("BuildingFolder").transform;
                    Instantiate<GameObject>(placeable, folder);
                    // hide card deck
                    GameObject.FindGameObjectWithTag("DeckFolder").GetComponent<DeckController>().HideDeck();

                    // comfirm place building
                    //      call remove()
                    // cancel place building
                    //      destroy placeable
                    //      show card deck
                    break;
                case CardType.Event:
                    break;
                default:
                    break;
            }
        }
        
    }

    /// <summary>
    /// activate hebaviour when card is in deck
    /// </summary>
    public void Activate()
    {
        priceDisplayer.SetActive(false);
        animator.enabled = true;
        bringToFront.enabled = true;
        bringToFront.Initialise();
        GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    /// <summary>
    /// called by placeable object when cancel to place building
    /// </summary>
    public void OnPlaceCancelled()
    {
        // show card deck
    }

    /// <summary>
    /// called by placeable object when confirm to place building
    /// </summary>
    public void OnPlace()
    {
        // remove self from deck
        // destroy self
        Destroy(gameObject);
    }

    public void SetInteractable(bool value)
    {
        button.enabled = value;
        animator.enabled = value;
    }

    private void OnDestroy()
    {
        if (inShop)
        {

        }
        else
        {
            GameObject deck = GameObject.FindGameObjectWithTag("DeckFolder");
            if (deck != null)
            {
                deck.GetComponent<DeckController>().FanCards();
            }
        }
    }

    
}
