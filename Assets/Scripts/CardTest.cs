using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Perform testing on instantiating card, placeable and building
/// </summary>
public class CardTest : MonoBehaviour
{
    public GameObject card;
    public Transform folder;

    // Start is called before the first frame update
    void Start()
    {
        Card _card = new Card("Building02", "Builds building02", CardType.Building, 0);
        _card.buildingID = "r02";
        GameObject newcard = GameObject.Instantiate(card, folder);
        newcard.GetComponent<CardBehaviour>().SetCardInfo(_card);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
