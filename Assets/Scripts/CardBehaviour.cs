using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// Initialize child objects base on given card info
    /// </summary>
    /// <param name="card"></param>
    public void SetCardInfo(Card card)
    {
        this.card = card;
        Transform _cardButton = transform.Find("CardButton");
        GameObject _title = _cardButton.Find("Title").gameObject;
        _title.GetComponent<Text>().text = card.title;

        GameObject _description = _cardButton.Find("Description").gameObject;
        _description.GetComponent<Text>().text = card.description;

        if (card.type == CardType.Building)
        {
            GameObject _image = _cardButton.Find("Image").gameObject;
            _image.GetComponent<Image>().sprite = ModelManager.LoadImage(((BuildingCard)card).buildingID);
        }
        if (card.type == CardType.Event)
        {

        }
        transform.Find("CardButton").GetComponent<Button>().interactable = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (card == null)
        {
            Card _card = new Card("Test", "Some text", CardType.Building);
            SetCardInfo(_card);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// OnClick is called when the card object attached with this script is clicked
    /// </summary>
    public void OnClick()
    {
        if (inShop)
        {
            DeckController deckController = GameObject.FindGameObjectWithTag("DeckFolder").GetComponent<DeckController>();
            deckController.InsertFromShop(gameObject);
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

    public void Remove()
    {
        // remove reference to this card from other objects
        // destroy self
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
