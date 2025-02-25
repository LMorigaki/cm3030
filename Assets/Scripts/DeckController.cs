using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    GameObject cardContainer;
    Behaviour selectable, animator;

    private void Start()
    {
        cardContainer = transform.Find("CardsContainer").gameObject;
        selectable = GetComponent<UnityEngine.UI.Selectable>();
        animator = GetComponent<Animator>();
    }

    public void ShowDeck()
    {
        cardContainer.SetActive(true);
        selectable.enabled = true;
        animator.enabled = true;
    }

    public void HideDeck()
    {
        cardContainer.SetActive(false);
        selectable.enabled = false;
        animator.enabled = false;
    }
}
