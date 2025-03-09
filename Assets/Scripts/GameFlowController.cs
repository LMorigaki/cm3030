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

    public Button BtnEndTurn;
    public GameObject BtnEndTurnObj;

    public Button BtnShop;
    public GameObject BtnShopObj;

    public GameObject EventCardDisplayer;

    public Button BtnTurnGuide;
    public GameObject TurnGuideObj;
    public TextMeshProUGUI TurnGuideText;

    public Button BtnSummary;
    public GameObject SummaryObj;
    public TextMeshProUGUI SummaryText;

    public Button BtnNewGame;
    public GameObject GameOverSummaryObj;
    public TextMeshProUGUI GameOverSummaryNumbers;
    public TextMeshProUGUI GameOverSummaryText;

    public Button BtnTutorial;
    public GameObject TutorialObj;
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
    const int maxTime = 60;
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
        BtnShopObj.SetActive(false);
        BtnEndTurnObj.SetActive(false);
    }

    private void Awake()
    {
        card = Resources.Load<GameObject>("Prefabs/CardButton");
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        ShowTutorial();
    }   

    public void ShowTutorial()
    {
        TutorialObj.SetActive(true);
        BtnTutorial.interactable = true;
    }

    public void OnBtnTutorialClicked()
    {
        BtnTutorial.interactable = false;
        TutorialObj.SetActive(false);
        ShowTurnGuide();
    }

    void ShowTurnGuide()
    {
        targetCash = GetNewTarget(currentTurn);
        TurnGuideObj.SetActive(true);
        BtnTurnGuide.interactable = true;
        TurnGuideText.text = "Own " + targetCash + " of cash at the end of this turn!";
    }

    public void OnBtnTurnGuideClicked()
    {
        BtnTurnGuide.interactable = false;
        TurnGuideObj.SetActive(false);
        OnTurnBegin();
    }

    /// <summary>
    /// called before turn begin<br/>
    /// initialise/update variables
    /// </summary>
    public void OnTurnBegin()
    {
        // update target cash
        UpdateTexts();
        
        BtnNextTurn.interactable = true;
        //BtnNextPharse.interactable = true;
        //BtnNextPharse.GetComponentInChildren<TextMeshProUGUI>().text = BtnNextPharseText.drawBuildings;
        //BtnNextPharseObj.SetActive(true);

        BtnShop.interactable = false;
        BtnShop.GetComponentInChildren<TextMeshProUGUI>().text = BtnShopText.showShop;
        BtnShopObj.SetActive(false);
        
        StartCoroutine(DrawCards());
    }

    /// <summary>
    /// display draw building cards animate
    /// </summary>
    /// <returns></returns>
    IEnumerator DrawCards()
    {
        // draw building cards
        deckController.ShowDeck();
        // define fixed types and amount of building at draw
        List<BuildingType> types = new List<BuildingType> 
        {
            BuildingType.Residential, BuildingType.Residential,
            BuildingType.Commercial, BuildingType.Commercial
        };
        for (int i = 0; i < deckController.maxCardCount; i++)
        {
            if (types.Count > 0)
            {
                deckController.InsertRandomCards(false, 1, types[0]);
                types.RemoveAt(0);
            }
            else
            {
                deckController.InsertRandomCards(false, 1);
            }
            deckController.FanCards();
            yield return new WaitForSeconds(0.25f);
        }
        deckController.HideDeck();
        yield return new WaitForSeconds(0.5f);
        BtnEndTurn.interactable = true;

        // draw event cards
        GameObject[] eventCards = new GameObject[2];
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
        yield return new WaitForSeconds(3);
        EventCardDisplayer.SetActive(false);
        for (int i = 0; i < eventCards.Length; i++)
        {
            Destroy(eventCards[i]);
            eventCards[i] = null;
        }

        InitialiseShop();
        OnBuildingPharseStart();
    }

    void OnBuildingPharseStart()
    {
        deckController.ShowDeck();
        deckController.EnableCards();
        BtnShopObj.SetActive(true);
        BtnEndTurnObj.SetActive(true);
        BtnShopObj.SetActive(true);
        BtnShop.interactable = true;
        // start timer
        timer = StartCoroutine(UpdateTime());

        StartCoroutine(OnBuildingPharseStartLate());
    }

    IEnumerator OnBuildingPharseStartLate()
    {
        // prevent multiple click on button
        yield return new WaitForSeconds(0.25f);
        BtnEndTurn.interactable = true;
    }

    void OnBuildingPharseEnd()
    {
        BtnEndTurn.interactable = false;
        BtnEndTurnObj.SetActive(false);
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
        BtnEndTurnObj.SetActive(false);
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
        BtnEndTurnObj.SetActive(true);
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

        SummaryText.text = GetSummary();
        GameOverSummaryNumbers.text = GetSummary();

        cash += (income - expenditure);
        UpdateTexts();
        if (cash < targetCash)
        {
            OnGameOver(false);
            return;
        }
        else
        {
            ShowSummary();
        }

        currentTurn++;

        if (currentTurn > maxTurns)
        {
            OnGameOver(true);
            return;
        }

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
        ShowTurnGuide();
    }

    void OnGameOver(bool win)
    {
        deckController.RemoveAll();

        if (win)
        {
            GameOverSummaryText.text = "You achieved all targets";
        }
        else
        {
            GameOverSummaryText.text = "You failed to reach the target";
        }
        GameOverSummaryObj.SetActive(true);
        BtnNewGame.interactable = true;
    }

    public void OnBtnNewGameClicked()
    {
        BtnNewGame.interactable = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// handle onclick event of next pharse button<br/>
    /// handles 
    /// </summary>
    public void OnBtnNextPharseClicked()
    {
        BtnEndTurn.interactable = false;
        OnBuildingPharseEnd();
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

    string GetSummary()
    {
        return currentTurn + "/" + maxTurns + "\r\n" +
               "\r\n" +
                cash + "\r\n" +
                income + "\r\n" +
                expenditure + "\r\n" +
                (income - expenditure) + "\r\n" +
                "\r\n" +
                (cash + (income - expenditure)) + "/" + targetCash;
    }

    /// <summary>
    /// calculate and return new income target 
    /// </summary>
    int GetNewTarget(int turn)
    {
        return targetCash = Mathf.FloorToInt(targetCash * 1.15f);
    }
}
