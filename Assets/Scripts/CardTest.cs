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
        for (int i = 0; i < 5; i++)
        {
            Card _card = Card.RandomBuildingCard(BuildingType.Residential);
            GameObject newcard = GameObject.Instantiate(card, folder);
            newcard.GetComponent<CardBehaviour>().SetCardInfo(_card);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
