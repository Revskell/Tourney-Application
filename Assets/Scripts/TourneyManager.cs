using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TourneyManager : MonoBehaviour
{

    private Tourney tourney;
    [SerializeField] public GameObject createMenuContainer;
    [SerializeField] public GameObject scenarioContainer;
    [SerializeField] public GameObject playersContainer;

    public string tourneyName;
    public int numberOfRounds;
    public int numberOfPlayers;

    public List<string> scenarioList = new List<string>();
    public List<List<string>> playerList = new List<List<string>>();

    public int currentRound;

    [SerializeField] public TextMeshProUGUI tourneyTitleText;
    [SerializeField] public TextMeshProUGUI scenarioText;
    [SerializeField] public TextMeshProUGUI roundText;

    [SerializeField] public GameObject pairingsContainer;
    [SerializeField] public GameObject pairingsTextPrefab;
    [SerializeField] public GameObject gamesContainerPrefab;
    [SerializeField] public GameObject spacerPrefab;

    [SerializeField] public TextMeshProUGUI alertText;

    public void StartTourney()
    {
        Clear();

        this.tourney = CreateTourney();
        this.currentRound = 1;
        this.tourney.FillRankedPlayerList();

        tourneyTitleText.text = tourneyName;
        scenarioText.text = scenarioList[currentRound - 1];
        roundText.text = "Round " + currentRound;

        CreateContainers();
        tourney.CreateRound(currentRound, scenarioList[currentRound - 1]);
        FillContainers(tourney.roundList[0]);
    }

    private void Clear()
    {
        for (int i = pairingsContainer.transform.childCount - 1; i >= 0; i--) Destroy(pairingsContainer.transform.GetChild(i).gameObject);
        scenarioList.Clear();
        playerList.Clear();
    }

    private Tourney CreateTourney()
    {
        string tName = createMenuContainer.GetComponentInChildren<TMP_InputField>().text;
        if (string.IsNullOrEmpty(tName)) tName = "Tourney";
        this.tourneyName = tName;

        TMP_Dropdown[] dropdowns = createMenuContainer.GetComponentsInChildren<TMP_Dropdown>();

        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            string dropdownName = dropdown.gameObject.name;

            if (dropdownName == "DropdownRounds") numberOfRounds = int.Parse(dropdown.options[dropdown.value].text);
            else if (dropdownName == "DropdownPlayers") numberOfPlayers = int.Parse(dropdown.options[dropdown.value].text);
        }

        TMP_Dropdown[] dropdownScenarios = scenarioContainer.GetComponentsInChildren<TMP_Dropdown>();
        if (dropdownScenarios != null)
        {
            foreach (TMP_Dropdown dropdownScenario in dropdownScenarios) scenarioList.Add(dropdownScenario.options[dropdownScenario.value].text);
        }

        TMP_InputField[] inputFields = playersContainer.GetComponentsInChildren<TMP_InputField>();
        List<string> player = new List<string>();
        int counter = 0;
        int playerCounter = 1;
        foreach (TMP_InputField inputField in inputFields)
        {
            string inputFieldName = inputField.gameObject.name;

            if (inputFieldName == "InputName")
            {
                player = new List<string>();
                string name = inputField.text;
                if (string.IsNullOrEmpty(name)) name = "Player " + playerCounter;
                player.Add(name);
                counter++;
            }
            if (inputFieldName == "InputNickname")
            {
                string nickName = inputField.text;
                if (string.IsNullOrEmpty(nickName)) nickName = "Nickname " + playerCounter;
                player.Add(nickName);
                counter++;
            }
            if (counter == 2)
            {
                counter = 0;
                playerCounter++;
                string parentName = inputField.transform.parent.gameObject.name;
                if (parentName.Contains("Good")) player.Add("Good");
                else if (parentName.Contains("Evil")) player.Add("Evil");
                playerList.Add(player);
            }
        }

        return new Tourney(tourneyName, numberOfRounds, scenarioList, numberOfPlayers, playerList);
    }

    private void CreateContainers()
    {
        GameObject pairingsContainer = GameObject.Find("PairingsContainer");
        for (int i = 0; i < numberOfPlayers / 2; i++)
        {
            Instantiate(pairingsTextPrefab, pairingsContainer.transform);
            Instantiate(gamesContainerPrefab, pairingsContainer.transform);
            Instantiate(spacerPrefab, pairingsContainer.transform);
        }
    }

    public void FillContainers(Round round)
    {
        List<Player> players = GetPlayers(round);
        List<GameObject> pairingsTexts = GetContainers("PairingsText(Clone)");
        List<GameObject> gamesContainers = GetContainers("GamesContainer(Clone)");

        int currentPlayerIndex = 0;
        
        for (int i = 0; i < pairingsTexts.Count; i++)
        {
            Player goodPlayer = players[currentPlayerIndex];
            Player evilPlayer = players[currentPlayerIndex + 1];

            string pairingsTextString = $"{goodPlayer.name} \"{goodPlayer.nickname}\" vs {evilPlayer.name} \"{evilPlayer.nickname}\"";
            pairingsTexts[i].GetComponent<TextMeshProUGUI>().text = pairingsTextString;

            currentPlayerIndex += 2;

            if (currentPlayerIndex >= numberOfPlayers)
                currentPlayerIndex = 0;
        }

        int currentGameNumber = 1;
        foreach (GameObject gamesContainer in gamesContainers)
        {
            string gameTextString = "Game " + currentGameNumber;
            gamesContainer.transform.Find("GamesText").GetComponent<TextMeshProUGUI>().text = gameTextString;

            gamesContainer.transform.Find("GoodPlayer").GetComponent<TextMeshProUGUI>().text = players[currentPlayerIndex].name;
            gamesContainer.transform.Find("EvilPlayer").GetComponent<TextMeshProUGUI>().text = players[currentPlayerIndex + 1].name;

            currentPlayerIndex += 2;
            currentGameNumber++;
            if (currentPlayerIndex >= numberOfPlayers) currentPlayerIndex = 0;
        }
    }

    private List<Player> GetPlayers(Round round)
    {
        List<Player> players = new List<Player>();

        for (int i = 0; i < numberOfPlayers / 2; i++)
        {
            players.Add(round.gameList[i].goodPlayer);
            players.Add(round.gameList[i].evilPlayer);
        }

        return players;
    }

    private List<GameObject> GetContainers(string containerName)
    {
        List<GameObject> containers = new List<GameObject>();

        Transform[] allChildren = pairingsContainer.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            if (child.name.Equals(containerName)) containers.Add(child.gameObject);
        }

        return containers;
    }

    public bool AreAllInputsFilled()
    {
        bool result = true;

        foreach (TMP_InputField input in pairingsContainer.GetComponentsInChildren<TMP_InputField>())
        {
            if (string.IsNullOrEmpty(input.text))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    public void ClearAllInputs()
    {
        foreach (TMP_InputField input in pairingsContainer.GetComponentsInChildren<TMP_InputField>())
        {
            input.text = string.Empty;
        }
    }

    public void NextRound()
    {
        if (AreAllInputsFilled())
        {
            alertText.color = Color.white;
            alertText.text = "";

            fillGamePoints();
            tourney.RankPlayers(currentRound-1);

            if (currentRound >= numberOfRounds) MenuManager.Instance.ShowTourneyResultsMenu(tourney, true);
            else
            {
                this.currentRound++;
                tourney.CreateRound(currentRound, scenarioList[currentRound - 1]);
                scenarioText.text = scenarioList[currentRound - 1];
                roundText.text = "Round " + currentRound;
                ClearAllInputs();
                FillContainers(tourney.roundList[currentRound - 1]);
            }
        }
        else
        {
            alertText.color = Color.red;
            alertText.text = "Fill all the inputs first";
        }
    }

    private List<Points> getGamePoints()
    {
        List<GameObject> containers = GetContainers("GamesContainer(Clone)");

        List<Points> pointsList = new List<Points>();

        for (int i = 0; i < containers.Count; i++)
        {
            GameObject gameContainer = containers[i];
            Points points = new Points();

            points.goodGainedVP = int.Parse(gameContainer.transform.Find("GoodPlayer/GainedGoodVPInput").GetComponent<TMP_InputField>().text);
            points.goodLostVP = int.Parse(gameContainer.transform.Find("GoodPlayer/LostGoodVPInput").GetComponent<TMP_InputField>().text);
            points.evilGainedVP = int.Parse(gameContainer.transform.Find("EvilPlayer/GainedEvilVPInput").GetComponent<TMP_InputField>().text);
            points.evilLostVP = int.Parse(gameContainer.transform.Find("EvilPlayer/LostEvilVPInput").GetComponent<TMP_InputField>().text);

            TMP_Dropdown leaderKilledGoodDropdown = gameContainer.transform.Find("GoodPlayer/LeaderKilledGoodDropdown").GetComponent<TMP_Dropdown>();
            TMP_Dropdown leaderKilledEvilDropdown = gameContainer.transform.Find("EvilPlayer/LeaderKilledEvilDropdown").GetComponent<TMP_Dropdown>();

            points.goodHasKilledLeader = leaderKilledGoodDropdown.options[leaderKilledGoodDropdown.value].text == "Yes";
            points.evilHasKilledLeader = leaderKilledEvilDropdown.options[leaderKilledEvilDropdown.value].text == "Yes";

            pointsList.Add(points);
        }

        return pointsList;
    }

    private void fillGamePoints()
    {
        List<Points> points = getGamePoints();

        int counter = 0;
        foreach(Game game in tourney.roundList[currentRound-1].gameList)
        {
            game.gamePoints.goodGainedVP = points[counter].goodGainedVP;
            game.gamePoints.goodLostVP = points[counter].goodLostVP;
            game.gamePoints.goodHasKilledLeader = points[counter].goodHasKilledLeader;

            game.gamePoints.evilGainedVP = points[counter].evilGainedVP;
            game.gamePoints.evilLostVP = points[counter].evilLostVP;
            game.gamePoints.evilHasKilledLeader = points[counter].evilHasKilledLeader;

            counter++;
        }
    }
}