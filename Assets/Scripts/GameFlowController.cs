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
    public Button BtnNextPharse;
    public GameObject BtnNextPharseObj;
    public Button BtnShop;
    public GameObject BtnShopObj;

    public TilemapController tilemapController;
    public DeckController deckController;
    public ShopController shopController;

    static class BtnNextPharseText
    {
        public static readonly string drawBuildings = "Draw Buildings";
        public static readonly string drawEvents = "Draw Events";
        public static readonly string returnCards = "Return Cards";
    }
    static class BtnShopText
    {
        public static readonly string showShop = "Show Shop";
        public static readonly string hideShop = "Hide Shop";
    }

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
        // update target cash
        targetCash = GetNewTarget(currentTurn);
        BtnNextTurn.interactable = true;
        // collect income from building
        int profit = Mathf.FloorToInt(tilemapController.board.TotalProfit());
        cash += profit;
        // update GUI texts
        UpdateTexts();
        // start timer
        timer = StartCoroutine(UpdateTime());

        BtnNextPharse.interactable = true;
        BtnNextPharse.GetComponentInChildren<TextMeshProUGUI>().text = BtnNextPharseText.drawBuildings;
        BtnShop.interactable = false;
        BtnShop.GetComponentInChildren<TextMeshProUGUI>().text = BtnShopText.showShop;
        BtnShopObj.SetActive(false);
        
        //StartCoroutine(DrawBuildingCards());
    }

    IEnumerator DrawBuildingCards()
    {
        deckController.ShowDeck();
        for (int i = 0; i < deckController.maxCardCount; i++)
        {
            deckController.InsertRandomCards(false, 1);
            deckController.FanCards();
            yield return new WaitForSeconds(0.25f);
        }
        deckController.HideDeck();
        yield return new WaitForSeconds(0.5f);
        BtnNextPharse.interactable = true;
    }

    void DrawEventCards()
    {
        // todo: draw event cards
    }

    void OnBuildingPharseStart()
    {
        deckController.ShowDeck();
        deckController.EnableCards();
        BtnShopObj.SetActive(true);
        BtnShop.interactable = true;
        StartCoroutine(OnBuildingPharseStartLate());
    }

    IEnumerator OnBuildingPharseStartLate()
    {
        yield return new WaitForSeconds(0.25f);
        BtnNextPharse.interactable = true;
    }

    void OnBuildingPharseEnd()
    {
        BtnNextPharse.interactable = false;
        BtnNextPharseObj.SetActive(false);
        BtnShop.interactable = false;
        BtnShopObj.SetActive(false);
        deckController.RemoveAll();
        deckController.HideDeck();
        ClearShop();
        OnTurnEnd();
    }

    void InitialiseShop()
    {
        shopController.gameObject.SetActive(true);
        shopController.InsertRandomCards();
        shopController.gameObject.SetActive(false);
    }

    void ShowShop()
    {
        Debug.Log("show shop");
        BtnNextPharseObj.SetActive(false);
        shopController.gameObject.SetActive(true);
    }

    void HideShop()
    {
        shopController.gameObject.SetActive(false);
        BtnNextPharseObj.SetActive(true);
    }

    void ClearShop()
    {
        shopController.RemovelAll();
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
        StopAllCoroutines();

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

    void OnGameOver()
    {
        BtnNextTurn.interactable = false;
        StopAllCoroutines();
        deckController.RemoveAll();

        // todo: disable GUI interactions
    }

    public void OnBtnNextPharseClicked()
    {
        BtnNextPharse.interactable = false;
        TextMeshProUGUI textMesh = BtnNextPharse.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh.text == BtnNextPharseText.drawBuildings)
        {
            textMesh.text = BtnNextPharseText.drawEvents;
            StartCoroutine(DrawBuildingCards());
        }
        else if (textMesh.text == BtnNextPharseText.drawEvents)
        {
            textMesh.text = BtnNextPharseText.returnCards;
            DrawEventCards();
            InitialiseShop();
            OnBuildingPharseStart();
        }
        else if (textMesh.text == BtnNextPharseText.returnCards)
        {
            OnBuildingPharseEnd();
        }
    }

    public void OnBtnShopClicked()
    {
        BtnShop.interactable = false;
        TextMeshProUGUI textMesh = BtnShop.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh.text == BtnShopText.showShop)
        {
            textMesh.text = BtnShopText.hideShop;
            ShowShop();
        }
        else if (textMesh.text == BtnShopText.hideShop)
        {
            textMesh.text = BtnShopText.showShop;
            HideShop();
        }
        StartCoroutine(OnBtnShopClickedLate());
    }

    IEnumerator OnBtnShopClickedLate()
    {
        yield return new WaitForSeconds(0.25f);
        BtnShop.interactable = true;
    }

    /// <summary>
    /// called when a building is placed/removed on board
    /// </summary>
    public void OnBoardChange()
    {
        income = Mathf.FloorToInt(tilemapController.board.TotalProfit());
        expenditure = Mathf.FloorToInt(tilemapController.board.TotalUpkeep());

        UpdateTexts();
        //execution wait after objects are destroyed
        StartCoroutine(OnBoardChangeLate());
    }

    IEnumerator OnBoardChangeLate()
    {
        yield return new WaitForEndOfFrame();
        if (!deckController.HasCard())
        {
            OnBuildingPharseEnd();
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

    /// <summary>
    /// updates GUI textes
    /// </summary>
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
