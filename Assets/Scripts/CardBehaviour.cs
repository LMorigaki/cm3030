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
        transform.Find("CardButton").GetComponent<Button>().interactable = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (card == null)
        {
            Card _card = new Card("Test", "Some text", CardType.Building, 0);
            SetCardInfo(_card);
        }
        
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
        // On click handle starts here
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
}
