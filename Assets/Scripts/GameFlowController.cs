using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFlowController : MonoBehaviour
{
    #region GUI elements
    public TextMeshProUGUI TurnText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI CashText;
    public TextMeshProUGUI TargetText;
    public TextMeshProUGUI IncomeText;
    public Button BtnNextTurn;
    public Button BtnNextPharse;
    public GameObject BtnNextPharseObj;
    public Button BtnShop;
    public GameObject BtnShopObj;
    public GameObject EventCardDisplayer;
    public Button BtnSummary;
    public GameObject SummaryObj;
    public TextMeshProUGUI SummaryText;
    #endregion

    public TilemapController tilemapController;
    public DeckController deckController;
    public ShopController shopController;

    static class BtnNextPharseText
    {
        public static readonly string drawBuildings = "Draw Buildings";
        public static readonly string drawEvents = "Draw Events";
        public static readonly string endTurn = "End Turn";
    }
    static class BtnShopText
    {
        public static readonly string showShop = "Show Shop";
        public static readonly string hideShop = "Hide Shop";
    }

    /// <summary>
    /// card prefab
    /// </summary>
    GameObject card;

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
        targetCash = cash;
        UpdateTexts();
        TurnText.text = "Round: " + currentTurn + "/" + maxTurns;
    }

    private void Awake()
    {
        card = Resources.Load<GameObject>("Prefabs/CardButton");
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        OnTurnBegin();
    }   

    /// <summary>
    /// called before turn begin<br/>
    /// initialise/update variables
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
        UpdateTexts();
        // start timer
        timer = StartCoroutine(UpdateTime());
        BtnNextTurn.interactable = true;
        BtnNextPharse.interactable = true;
        BtnNextPharse.GetComponentInChildren<TextMeshProUGUI>().text = BtnNextPharseText.drawBuildings;
        BtnNextPharseObj.SetActive(true);
        BtnShop.interactable = false;
        BtnShop.GetComponentInChildren<TextMeshProUGUI>().text = BtnShopText.showShop;
        BtnShopObj.SetActive(false);
        
        //StartCoroutine(DrawBuildingCards());
    }

    /// <summary>
    /// display draw building cards animate
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// display draw event cards animate<br/>
    /// then go to play building cards pharse
    /// </summary>
    IEnumerator DrawEventCards()
    {
        // improve: animation of drawing event cards

        GameObject[] eventCards = new GameObject[3];
        EventCardDisplayer.SetActive(true);
        for (int i = 0; i < eventCards.Length; i++)
        {
            EventCard _card = EventCard.RandomEventCard();
            eventCards[i] = GameObject.Instantiate(card, EventCardDisplayer.transform);
            eventCards[i].GetComponent<CardBehaviour>().SetCardInfo(_card);
            eventCards[i].GetComponent<CardBehaviour>().SetInteractable(false);
            tilemapController.board.ApplyEvent(_card);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(1);
        EventCardDisplayer.SetActive(false);
        for (int i = 0; i < eventCards.Length; i++)
        {
            Destroy(eventCards[i]);
            eventCards[i] = null;
        }
        OnBuildingPharseStart();
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
        // prevent multiple click on button
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
        BtnNextPharseObj.SetActive(false);
        shopController.gameObject.SetActive(true);
    }

    public bool BuyCard(int price)
    {
        if (cash >= price)
        {
            cash -= price;
            UpdateTexts();
            return true;
        }
        else
        {
            return false;
        }
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

        SummaryText.text = currentTurn + "/" + maxTurns + "\r\n" +
            "\r\n" +
             cash + "\r\n" +
             income + "\r\n" +
             expenditure + "\r\n" +
             (income - expenditure) + "\r\n" +
             "\r\n" +
             (cash + (income - expenditure)) + "/" + targetCash;

        cash += (income - expenditure);
        UpdateTexts();
        if (cash < targetCash)
        {
            OnGameOver();
            return;
        }
        else
        {
            ShowSummary();
        }

        currentTurn++;
        tilemapController.board.StepAndRemoveEventBonus();
        TurnText.text = "Round: " + currentTurn + "/" + maxTurns;
    }

    void ShowSummary()
    {
        SummaryObj.SetActive(true);
        BtnSummary.interactable = true;
    }

    public void OnBtnSummaryClicked()
    {
        BtnSummary.interactable = false;
        SummaryObj.SetActive(false);
        OnTurnBegin();
    }

    void OnGameOver()
    {
        BtnNextTurn.interactable = false;
        StopAllCoroutines();
        deckController.RemoveAll();

        // todo: disable GUI interactions
        // todo: show dialog about game summary
    }

    /// <summary>
    /// handle onclick event of next pharse button<br/>
    /// handles 
    /// </summary>
    public void OnBtnNextPharseClicked()
    {
        BtnNextPharse.interactable = false;
        TextMeshProUGUI textMesh = BtnNextPharse.GetComponentInChildren<TextMeshProUGUI>();
        // go to draw building cards pharse
        if (textMesh.text == BtnNextPharseText.drawBuildings)
        {
            textMesh.text = BtnNextPharseText.drawEvents;
            StartCoroutine(DrawBuildingCards());
        }
        // go to draw event cards pharse
        // then go to play building cards pharse
        else if (textMesh.text == BtnNextPharseText.drawEvents)
        {
            textMesh.text = BtnNextPharseText.endTurn;
            StartCoroutine(DrawEventCards());
            InitialiseShop();
        }
        // end play building cards pharse
        else if (textMesh.text == BtnNextPharseText.endTurn)
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
        // prevent multiple click on button
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
    }

    /// <summary>
    /// initialize and run timer, ends turn when time is up
    /// </summary>
    IEnumerator UpdateTime()
    {
        time = maxTime;
        while (time > 0)
        {
            TimeText.text = "End Round (" + time + "s)";
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
        // improve: show change of value in animation
        CashText.text = "Cash: " + cash;
        TargetText.text = "Target: " + targetCash;
        IncomeText.text = "Income: " + (income - expenditure);
    }

    /// <summary>
    /// calculate and return new income target 
    /// </summary>
    int GetNewTarget(int turn)
    {
        return targetCash = targetCash + Mathf.FloorToInt( 110 * (turn + 0.5f) );
    }
}
