using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFlowController : MonoBehaviour
{
    public TextMeshProUGUI DisplayTurn;
    public TextMeshProUGUI DisplayTime;
    public TextMeshProUGUI DisplayCash;
    public TextMeshProUGUI DisplayTargetIncome;
    public TextMeshProUGUI DisplayIncome;
    public TextMeshProUGUI DisplayExpenditure;
    public Button BtnNextTurn;

    public TilemapController tilemapController;
    public DeckController deckController;
    public ShopController shopController;

    int currentTurn;
    const int maxTurns = 5;
    /// <summary>
    /// remaining time of this turn
    /// </summary>
    int time;
    const int maxTime = 120;
    /// <summary>
    /// the cash in hand
    /// </summary>
    int cash;
    /// <summary>
    /// the expected income will be obtained at the begin of turn
    /// </summary>
    int income;
    /// <summary>
    /// the target amount of cash need to be meet at the end of turn
    /// </summary>
    int targetCash;
    /// <summary>
    /// the expected expenditure will be taken at the end of turn
    /// </summary>
    int expenditure;
    Coroutine timer;

    void Initialize()
    {
        currentTurn = 1;
        cash = 1000;
        UpdateTexts();
        DisplayTurn.text = "Round: " + currentTurn + "/" + maxTurns;
        deckController.InsertRandomCards();
        deckController.FanCards();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        OnTurnBegin();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// called before turn begin
    /// </summary>
    public void OnTurnBegin()
    {
        if (currentTurn > maxTurns)
        {
            OnGameOver();
            return;
        }
        targetCash = GetNewTarget(currentTurn);
        BtnNextTurn.interactable = true;
        
        UpdateTexts();
        timer = StartCoroutine(UpdateTime());
        // todo: draw event card
        StartBuildingPharse();
    }

    /// <summary>
    /// ends current turn
    /// </summary>
    public void EndTurn()
    {
        BtnNextTurn.interactable = false;
        OnTurnEnd();
    }

    /// <summary>
    /// called when turn ends
    /// </summary>
    public void OnTurnEnd()
    {
        BtnNextTurn.interactable = false;
        StopCoroutine(timer);

        deckController.RemoveAll();

        cash -= Mathf.FloorToInt(tilemapController.board.TotalUpkeep());
        if (cash < targetCash)
        {
            OnGameOver();
            return;
        }

        currentTurn++;
        tilemapController.board.StepAndRemoveEventBonus();
        DisplayTurn.text = "Round: " + currentTurn + "/" + maxTurns;
        OnTurnBegin();
    }

    void StartBuildingPharse()
    {
        deckController.ShowDeck();
        deckController.InsertRandomCards();
        deckController.FanCards();

        int profit = Mathf.FloorToInt(tilemapController.board.TotalProfit());
        cash += profit;

        UpdateTexts();
    }

    void EndBuildingPharse()
    {
        deckController.RemoveAll();
        deckController.HideDeck();

        StartShoppingParse();
    }

    void StartShoppingParse()
    {
        shopController.gameObject.SetActive(true);
        shopController.InsertRandomCards();
    }

    public void EndShoppingParse()
    {
        shopController.RemovelAll();
        shopController.gameObject.SetActive(false);
    }

    void OnGameOver()
    {
        // todo: disable GUI interactions
    }

    /// <summary>
    /// called when a building is placed/removed on board
    /// </summary>
    public void OnBoardChange()
    {
        income = Mathf.FloorToInt(tilemapController.board.TotalProfit());
        expenditure = Mathf.FloorToInt(tilemapController.board.TotalUpkeep());

        UpdateTexts();

        StartCoroutine(OnBoardChangeLate());
    }

    IEnumerator OnBoardChangeLate()
    {
        yield return new WaitForEndOfFrame();
        if (!deckController.HasCard())
        {
            EndBuildingPharse();
        }
    }

    /// <summary>
    /// initialize and run timer, ends turn when time is up
    /// </summary>
    IEnumerator UpdateTime()
    {
        time = maxTime;
        while (time > 0)
        {
            DisplayTime.text = "End Round (" + time + "s)";
            yield return new WaitForSeconds(1);
            time--;
        }
        OnTurnEnd();
    }

    void UpdateTexts()
    {
        DisplayCash.text = "Cash: " + cash;
        DisplayTargetIncome.text = "Target: " + targetCash;
        DisplayIncome.text = "Income: " + income;
        DisplayExpenditure.text = "Expenses: " + expenditure;
    }

    /// <summary>
    /// calculate and return new income target 
    /// </summary>
    int GetNewTarget(int turn)
    {
        return targetCash = 200 + turn * 50;
    }
}
