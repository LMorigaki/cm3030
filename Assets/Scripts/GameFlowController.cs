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

    int currentTurn;
    const int maxTurns = 5;
    /// <summary>
    /// remaining time of this turn
    /// </summary>
    int time;
    const int maxTime = 120;
    int cash, income, targetIncome, expenditure;
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
        targetIncome = GetNewTarget(currentTurn);
        BtnNextTurn.interactable = true;
        UpdateTexts();
        deckController.FanCards();
        timer = StartCoroutine(UpdateTime());
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
        currentTurn++;
        tilemapController.board.StepAndRemoveEventBonus();
        DisplayTurn.text = "Round: " + currentTurn + "/" + maxTurns;
        OnTurnBegin();
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
        DisplayTargetIncome.text = "Target: " + (income - expenditure) + "/" + targetIncome;
        DisplayIncome.text = "Income: " + income;
        DisplayExpenditure.text = "Expenses: " + expenditure;
    }

    /// <summary>
    /// calculate and return new income target 
    /// </summary>
    int GetNewTarget(int turn)
    {
        return targetIncome = 200 + turn * 50;
    }
}
