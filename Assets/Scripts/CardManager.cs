using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardManager : MonoBehaviour
{
    public GameObject BuildingCardTemplate;
    public GameObject EventCardTemplate;
    public GameObject CardsArea;
    public GameObject BuildingGrid;

    // References to other managers
    private GameManager gameManager;
    //private GridManager gridManager;
    
    // For storing references to active cards in the CardsArea only
    List<GameObject> cardsInHand = new List<GameObject>();

    // Building card texts representing different building types from design document
    string[] buildingTexts = new string[] {
        "Residential Zone\nIncome: 20\nUpkeep: 10",
        "Commercial Zone\nIncome: 30\nUpkeep: 15",
        "Industrial Zone\nIncome: 40\nUpkeep: 20",
        "Park\nIncome: 5\nUpkeep: 5\n+10 to adjacent Residential",
        "School\nIncome: 0\nUpkeep: 25\n+15 to adjacent Residential",
        "Hospital\nIncome: 10\nUpkeep: 30\n+10 to adjacent zones",
        "Police Station\nIncome: 0\nUpkeep: 20\n+5 to all zones",
        "Fire Station\nIncome: 0\nUpkeep: 20\n+5 to all zones",
        "Power Plant\nIncome: 0\nUpkeep: 40\n+20 to Industrial",
        "Shopping Mall\nIncome: 50\nUpkeep: 30\n+15 to Commercial",
        "Office Building\nIncome: 35\nUpkeep: 25\n+10 to Commercial",
        "Factory\nIncome: 45\nUpkeep: 35\n+15 to Industrial"
    };

    // Event card texts representing random events from design document
    string[] eventTexts = new string[] {
        "Economic Boom\nIncome +20% this turn",
        "Budget Cuts\nExpenses -10% this turn",
        "Natural Disaster\n-30 income this turn",
        "Population Growth\n+5 to all Residential zones",
        "Tourism Boost\n+10 to all Commercial zones",
        "Factory Strike\n-15 to all Industrial zones"
    };

    // Keep track of which building types have been used
    private List<int> availableBuildingIndices = new List<int>();
    private List<int> availableEventIndices = new List<int>();

    void Start()
    {
        // Get references to managers
        gameManager = FindObjectOfType<GameManager>();
        //gridManager = FindObjectOfType<GridManager>();
        
        // Initialize available indices
        ResetAvailableCardIndices();
        
        // Draw initial cards
        DrawNewCards();
        
        // Log initial state
        Debug.Log("CardManager initialized. Cards area has " + CardsArea.transform.childCount + " cards.");
    }

    // Reset the available card indices (used when all cards have been drawn)
    private void ResetAvailableCardIndices()
    {
        availableBuildingIndices.Clear();
        for (int i = 0; i < buildingTexts.Length; i++)
        {
            availableBuildingIndices.Add(i);
        }
        
        availableEventIndices.Clear();
        for (int i = 0; i < eventTexts.Length; i++)
        {
            availableEventIndices.Add(i);
        }
    }

    // Draw a random unique card index
    private int GetRandomUniqueIndex(List<int> availableIndices)
    {
        if (availableIndices.Count == 0)
            return -1;
            
        int randomPosition = Random.Range(0, availableIndices.Count);
        int selectedIndex = availableIndices[randomPosition];
        availableIndices.RemoveAt(randomPosition);
        
        return selectedIndex;
    }

    // Draw new cards button handler
    public void DrawNewCards()
    {
        // Clear only cards in the CardsArea
        ClearCardsArea();
        
        Debug.Log("Drawing new cards. Grid has " + BuildingGrid.transform.childCount + " cards.");
        
        // Draw new cards
        // Spawn 4 building cards with random unique text
        for (int i = 0; i < 4; i++)
        {
            // Get random unique building text
            int randomIndex = GetRandomUniqueIndex(availableBuildingIndices);
            
            // If we ran out of unique building indices, reset the pool
            if (randomIndex == -1)
            {
                ResetAvailableCardIndices();
                randomIndex = GetRandomUniqueIndex(availableBuildingIndices);
            }
            
            string randomText = buildingTexts[randomIndex];
            
            // Instantiate card
            GameObject bCard = SpawnCard(BuildingCardTemplate, randomText);
        }

        // Spawn 1 event card with random unique text
        int randomEventIndex = GetRandomUniqueIndex(availableEventIndices);
        
        // If we ran out of unique event indices, reset the pool
        if (randomEventIndex == -1)
        {
            ResetAvailableEventIndices();
            randomEventIndex = GetRandomUniqueIndex(availableEventIndices);
        }
        
        string randomEventText = eventTexts[randomEventIndex];
        GameObject eCard = SpawnCard(EventCardTemplate, randomEventText);
        
        // Extract event type and apply its effect immediately
        string eventType = ExtractEventType(randomEventText);
        ApplyEventEffect(eventType);
        
        // After drawing new cards, recalculate the game economy
        if (gameManager != null)
        {
            gameManager.CalculateEconomy();
        }
    }

    // Helper method to spawn a card
    private GameObject SpawnCard(GameObject template, string text)
    {
        GameObject card = Instantiate(template, Vector3.zero, Quaternion.identity);
        card.transform.SetParent(CardsArea.transform, false);
        
        // Set the card text
        SetCardText(card, text);
        
        // Add DragDrop component if it doesn't exist
        /*if (card.GetComponent<DragDrop>() == null)
        {
            card.AddComponent<DragDrop>();
        }*/
        
        // Add to our list to track
        cardsInHand.Add(card);
        
        return card;
    }

    // Helper to extract event type
    private string ExtractEventType(string cardText)
    {
        if (string.IsNullOrEmpty(cardText))
            return "";
            
        int newlineIndex = cardText.IndexOf('\n');
        if (newlineIndex > 0)
        {
            return cardText.Substring(0, newlineIndex).Trim();
        }
        
        return cardText.Trim();
    }
    
    // Apply event effect
    public void ApplyEventEffect(string eventType)
    {
        if (gameManager == null) return;
        
        Debug.Log("Applying event effect: " + eventType);
        
        switch (eventType) {
            case "Economic Boom":
                // Increase income by 20%
                gameManager.ApplyIncomeModifier(1.2f);
                break;
            case "Budget Cuts":
                // Reduce expenses by 10%
                gameManager.ApplyUpkeepModifier(0.9f);
                break;
            case "Natural Disaster":
                // Reduce income by 30
                gameManager.ApplyFlatIncomeChange(-30);
                break;
            case "Population Growth":
                // Add +5 to all Residential zones
                gameManager.ApplyBonusToType("Residential Zone", 5);
                break;
            case "Tourism Boost":
                // Add +10 to all Commercial zones
                gameManager.ApplyBonusToType("Commercial Zone", 10);
                break;
            case "Factory Strike":
                // Reduce Industrial zones by 15
                gameManager.ApplyBonusToType("Industrial Zone", -15);
                break;
            default:
                Debug.LogWarning("Unknown event type: " + eventType);
                break;
        }
    }

    // Reset available event indices
    private void ResetAvailableEventIndices()
    {
        availableEventIndices.Clear();
        for (int i = 0; i < eventTexts.Length; i++)
        {
            availableEventIndices.Add(i);
        }
    }

    // Helper method to set text on a card
    void SetCardText(GameObject card, string text)
    {
        // If using TextMeshPro (recommended)
        TextMeshProUGUI textComponent = card.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            // Update text content
            textComponent.text = text;
            
            // Set text to be centered
            textComponent.alignment = TextAlignmentOptions.Center;
            
            // Make the text smaller
            textComponent.fontSize = 20;
        }
        else
        {
            // Legacy UI Text
            Text legacyText = card.GetComponentInChildren<Text>();
            if (legacyText != null)
            {
                // Update text content
                legacyText.text = text;
                
                // Set text to be centered
                legacyText.alignment = TextAnchor.MiddleCenter;
                
                // Make the text smaller
                legacyText.fontSize = 20;
            }
            else
            {
                Debug.LogWarning("No text component found on card: " + card.name);
            }
        }
    }
    
    // Method to only clear cards in the CardsArea
    void ClearCardsArea()
    {
        // Make sure we have cards to clear
        if (cardsInHand != null && cardsInHand.Count > 0)
        {
            // Destroy all cards in our list
            foreach (GameObject card in cardsInHand)
            {
                if (card != null && card.transform.parent == CardsArea.transform)
                {
                    Destroy(card);
                }
            }
            cardsInHand.Clear();
        }
        
        // For extra safety, directly clear all children of CardsArea
        foreach (Transform child in CardsArea.transform)
        {
            Destroy(child.gameObject);
        }
        
        Debug.Log("Cards area cleared. Now has " + CardsArea.transform.childCount + " cards.");
    }
    
    // Method to remove a card from our tracking list when it's moved to the grid
    public void RemoveCardFromHand(GameObject card)
    {
        if (cardsInHand.Contains(card))
        {
            cardsInHand.Remove(card);
            Debug.Log("Card removed from hand. Hand now has " + cardsInHand.Count + " cards.");
        }
    }
    
    // Check if a card is in the player's hand
    public bool IsCardInHand(GameObject card)
    {
        return cardsInHand.Contains(card);
    }
}