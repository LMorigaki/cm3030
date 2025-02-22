using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stores card info and provide behaviour of a card object
/// </summary>
public class CardInformation : MonoBehaviour
{
    /// <summary>
    /// Card info of this card object
    /// </summary>
    public Card card;

    /// <summary>
    /// Initialize child objects base on given card info
    /// </summary>
    /// <param name="card"></param>
    public void Initialize(Card card)
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
        Card _card = new Card("Test", "Some text", CardType.Building, 0);
        Initialize(_card);
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
    }
}
