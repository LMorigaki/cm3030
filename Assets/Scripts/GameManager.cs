using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Text DisplayTurn;
    public Text DisplayTime;
    public Text DisplayCash;
    public Text DisplayTarget;

    // Screen management
    public GameObject StartPanel;
    public GameObject GameplayElements;
    public Button StartButton;
    
    // Information cards
    public GameObject IncomeCard;
    public GameObject TimeCard;
    public GameObject TurnCard;
    public GameObject TargetCard;
    
    // Turn control
    public GameObject NextTurnButton;
    
    // Game state variables
    private int currentIncome = 0;
    private int currentUpkeep = 0;
    private int netIncome = 0;
    private int currentTurn = 1;
    private int maxTurns = 3; // Need to complete 3 turns to win
    private int turnTarget = 100; // Target income to reach
    private float gameTime = 300f; // 5 minutes in seconds for the entire game
    private bool gameActive = true;
    private bool turnComplete = false;
    private bool gameOver = false;
    
    // Event effects
    private float incomeModifier = 1.0f;
    private float upkeepModifier = 1.0f;
    //private Dictionary<string, int> typeBonuses = new Dictionary<string, int>();
    
    // Grid reference
    //public GameObject BuildingGrid;
    
    // References to other managers
    //private GridManager gridManager;
    //private CardManager cardManager;
    
    // Text components for each info card
    private TextMeshProUGUI incomeText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI turnText;
    private TextMeshProUGUI targetText;
    
    // Building data dictionary - maps building types to their properties
    //private Dictionary<string, BuildingData> buildingDataMap = new Dictionary<string, BuildingData>();
    
    // Class to store building properties
    [System.Serializable]
    public class BuildingData
    {
        public string name;
        public int baseIncome;
        public int upkeep;
        public string[] adjacencyBonuses; // Format: "TargetType:BonusAmount"
        
        public BuildingData(string name, int baseIncome, int upkeep, string[] adjacencyBonuses)
        {
            this.name = name;
            this.baseIncome = baseIncome;
            this.upkeep = upkeep;
            this.adjacencyBonuses = adjacencyBonuses;
        }
    }
    
    void Start()
    {
        // Initialize text components
        incomeText = IncomeCard.GetComponentInChildren<TextMeshProUGUI>();
        timeText = TimeCard.GetComponentInChildren<TextMeshProUGUI>();
        turnText = TurnCard.GetComponentInChildren<TextMeshProUGUI>();
        targetText = TargetCard.GetComponentInChildren<TextMeshProUGUI>();
        
        // Get reference to grid manager and card manager
        //gridManager = FindObjectOfType<GridManager>();
        //cardManager = FindObjectOfType<CardManager>();
        
        // Initialize building data
        //InitializeBuildingData();
        
        // Show start screen
        ShowStartScreen();
        
        // Set up start button listener
        if (StartButton != null)
        {
            StartButton.onClick.AddListener(StartGameFromButton);
        }
        
        // Update UI initially
        UpdateAllCards();
    }
    
    void Update()
    {
        if (gameActive && !turnComplete)
        {
            // Update game time
            gameTime -= Time.deltaTime;
            
            // Check if we've met the turn target
            if (netIncome >= turnTarget && !turnComplete)
            {
                // Successfully completed turn objective
                CompleteTurn();
            }
            
            // Check if time has run out
            if (gameTime <= 0)
            {
                // Game ended - check if we completed enough turns
                if (currentTurn > maxTurns)
                {
                    GameOver(true); // Success - completed all turns
                }
                else
                {
                    GameOver(false); // Failed - time ran out
                }
            }
            
            // Update time card
            UpdateTimeCard();
        }
    }
    
    // Screen management methods
    private void ShowStartScreen()
    {
        if (StartPanel != null) StartPanel.SetActive(true);
        if (GameplayElements != null) GameplayElements.SetActive(false);
    }
    
    public void StartGameFromButton()
    {
        if (StartPanel != null) StartPanel.SetActive(false);
        if (GameplayElements != null) GameplayElements.SetActive(true);
        
        StartGame();
    }
    
    private void InitializeBuildingData()
    {
        // Populate building data based on your building card texts
        /*
        buildingDataMap.Add("Residential Zone", new BuildingData("Residential Zone", 20, 10, new string[]{}));
        buildingDataMap.Add("Commercial Zone", new BuildingData("Commercial Zone", 30, 15, new string[]{}));
        buildingDataMap.Add("Industrial Zone", new BuildingData("Industrial Zone", 40, 20, new string[]{}));
        buildingDataMap.Add("Park", new BuildingData("Park", 5, 5, new string[]{"Residential Zone:10"}));
        buildingDataMap.Add("School", new BuildingData("School", 0, 25, new string[]{"Residential Zone:15"}));
        buildingDataMap.Add("Hospital", new BuildingData("Hospital", 10, 30, new string[]{"Residential Zone:10", "Commercial Zone:10", "Industrial Zone:10"}));
        buildingDataMap.Add("Police Station", new BuildingData("Police Station", 0, 20, new string[]{"All:5"}));
        buildingDataMap.Add("Fire Station", new BuildingData("Fire Station", 0, 20, new string[]{"All:5"}));
        buildingDataMap.Add("Power Plant", new BuildingData("Power Plant", 0, 40, new string[]{"Industrial Zone:20"}));
        buildingDataMap.Add("Shopping Mall", new BuildingData("Shopping Mall", 50, 30, new string[]{"Commercial Zone:15"}));
        buildingDataMap.Add("Office Building", new BuildingData("Office Building", 35, 25, new string[]{"Commercial Zone:10"}));
        buildingDataMap.Add("Factory", new BuildingData("Factory", 45, 35, new string[]{"Industrial Zone:15"}));
        */
    }
    
    public void CalculateEconomy()
    {
        int totalIncome = 0;
        int totalUpkeep = 0;
        
        /*if (gridManager == null)
        {
            Debug.LogError("GridManager reference not found!");
            return;
        }*/
        
        // Get all building cards in the grid
        //List<GameObject> buildingsInGrid = gridManager.GetAllBuildingsInGrid();
        
        // Calculate base income and upkeep for each building
        /*
        foreach (GameObject building in buildingsInGrid)
        {
            TextMeshProUGUI textComponent = building.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                string cardText = textComponent.text;
                string buildingType = ExtractBuildingType(cardText);
                
                if (buildingDataMap.TryGetValue(buildingType, out BuildingData data))
                {
                    int buildingIncome = data.baseIncome;
                    int buildingUpkeep = data.upkeep;
                    
                    // Apply type-specific bonuses from events
                    if (typeBonuses.ContainsKey(buildingType))
                    {
                        buildingIncome += typeBonuses[buildingType];
                    }
                    
                    totalIncome += buildingIncome;
                    totalUpkeep += buildingUpkeep;
                }
            }
        }
        */
        // Calculate adjacency bonuses
        /*
        foreach (GameObject building in buildingsInGrid)
        {
            TextMeshProUGUI textComponent = building.GetComponentInChildren<TextMeshProUGUI>();
            
            if (textComponent != null)
            {
                string cardText = textComponent.text;
                string buildingType = ExtractBuildingType(cardText);
                
                if (buildingDataMap.TryGetValue(buildingType, out BuildingData data))
                {
                    // Get adjacent buildings
                    List<GameObject> adjacentBuildings = gridManager.GetAdjacentBuildings(building);
                    
                    // Apply bonuses to adjacent buildings
                    foreach (string adjacencyBonus in data.adjacencyBonuses)
                    {
                        string[] parts = adjacencyBonus.Split(':');
                        if (parts.Length == 2)
                        {
                            string targetType = parts[0];
                            int bonusAmount = int.Parse(parts[1]);
                            
                            foreach (GameObject adjBuilding in adjacentBuildings)
                            {
                                TextMeshProUGUI adjText = adjBuilding.GetComponentInChildren<TextMeshProUGUI>();
                                if (adjText != null)
                                {
                                    string adjBuildingType = ExtractBuildingType(adjText.text);
                                    
                                    // Apply bonus if type matches or if bonus applies to all
                                    if (targetType == "All" || adjBuildingType == targetType)
                                    {
                                        totalIncome += bonusAmount;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        */

        // Apply modifiers
        totalIncome = Mathf.RoundToInt(totalIncome * incomeModifier);
        totalUpkeep = Mathf.RoundToInt(totalUpkeep * upkeepModifier);
        
        // Store the results
        currentIncome = totalIncome;
        currentUpkeep = totalUpkeep;
        netIncome = currentIncome - currentUpkeep;
        
        // Debug output
        Debug.Log($"Calculated: Income {currentIncome}, Upkeep {currentUpkeep}, Net {netIncome}, Target {turnTarget}");
        
        // Update income card
        UpdateIncomeCard();
    }
    
    // Extract building type from card text (takes first line of text)
    private string ExtractBuildingType(string cardText)
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
    
    // Start the game
    private void StartGame()
    {
        gameActive = true;
        turnComplete = false;
        gameOver = false;
        gameTime = 300f; // 5 minutes
        currentTurn = 1;
        
        // Reset modifiers
        ResetModifiers();
        
        // Set initial turn target
        turnTarget = 100 * currentTurn;
        
        // Disable next turn button
        NextTurnButton.GetComponent<Button>().interactable = false;
        
        UpdateTargetCard();
        
        // Clear grid and draw new cards
        ClearGrid();
        /*if (cardManager != null)
        {
            cardManager.DrawNewCards();
        }*/
    }
    
    // Reset modifiers for events
    private void ResetModifiers()
    {
        incomeModifier = 1.0f;
        upkeepModifier = 1.0f;
        //typeBonuses.Clear();
    }
    
    // Successfully complete a turn
    private void CompleteTurn()
    {
        turnComplete = true;
        NextTurnButton.GetComponent<Button>().interactable = true;
        
        // Display success message
        Debug.Log("Turn " + currentTurn + " completed successfully!");
        targetText.text = $"Target Met!\n{netIncome}/{turnTarget}\nClick Next Turn";
        
        // Check if all turns are complete
        if (currentTurn >= maxTurns)
        {
            GameOver(true);
        }
    }
    
    // Go to next turn
    public void NextTurn()
    {
        if (turnComplete && !gameOver)
        {
            currentTurn++;
            turnComplete = false;
            
            // Clear grid and economy
            ClearGrid();
            
            // Reset modifiers
            ResetModifiers();
            
            // Update turn target
            turnTarget = 100 * currentTurn;
            
            // Update UI
            UpdateTurnCard();
            UpdateTargetCard();
            
            // Disable next turn button
            NextTurnButton.GetComponent<Button>().interactable = false;
            
            // Draw new cards
            /*if (cardManager != null)
            {
                cardManager.DrawNewCards();
            }*/
        }
    }
    
    // Clear the grid for next turn
    private void ClearGrid()
    {
        // Destroy all buildings in the grid
        /*foreach (Transform child in BuildingGrid.transform)
        {
            Destroy(child.gameObject);
        }*/
        
        // Reset economy values
        currentIncome = 0;
        currentUpkeep = 0;
        netIncome = 0;
        
        // Update income card
        UpdateIncomeCard();
        
        // Update grid tracking
        /*if (gridManager != null)
        {
            gridManager.UpdateGridTracking();
        }*/
    }
    
    // Game over handler
    private void GameOver(bool success)
    {
        gameOver = true;
        gameActive = false;
        turnComplete = true;
        
        
        // Disable next turn button unless we've completed all turns
        NextTurnButton.GetComponent<Button>().interactable = false;
    }
    
    // Apply a percentage modifier to income
    public void ApplyIncomeModifier(float modifier)
    {
        incomeModifier = modifier;
        Debug.Log($"Income modifier set to {incomeModifier}");
        CalculateEconomy();
    }

    // Apply a percentage modifier to upkeep
    public void ApplyUpkeepModifier(float modifier)
    {
        upkeepModifier = modifier;
        Debug.Log($"Upkeep modifier set to {upkeepModifier}");
        CalculateEconomy();
    }

    // Apply a flat change to income
    public void ApplyFlatIncomeChange(int amount)
    {
        Debug.Log($"Applying flat income change: {amount}");
        currentIncome += amount;
        netIncome = currentIncome - currentUpkeep;
        UpdateIncomeCard();
    }

    // Apply a bonus to all buildings of a type
    public void ApplyBonusToType(string buildingType, int bonus)
    {
        //typeBonuses[buildingType] = bonus;
        Debug.Log($"Applied {bonus} bonus to {buildingType}");
        CalculateEconomy();
    }
    
    // Update methods for each info card
    private void UpdateIncomeCard()
    {
        if (incomeText != null)
        {
            incomeText.text = $"Income: {currentIncome}\nUpkeep: {currentUpkeep}\nNet: {netIncome}";
        }
    }
    
    private void UpdateTimeCard()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);
            timeText.text = $"Time\n{minutes:00}:{seconds:00}";
        }
    }
    
    private void UpdateTurnCard()
    {
        if (turnText != null)
        {
            turnText.text = $"Turn\n{currentTurn}/{maxTurns}";
        }
    }
    
    private void UpdateTargetCard()
    {
        if (targetText != null)
        {
            targetText.text = $"Target\n{turnTarget} Net Income";
        }
    }
    
    private void UpdateAllCards()
    {
        UpdateIncomeCard();
        UpdateTimeCard();
        UpdateTurnCard();
        UpdateTargetCard();
    }
    
}