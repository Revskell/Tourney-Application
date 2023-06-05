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
    public int numberOfGames;
    public int numberOfPlayers;

    public List<string> scenarioList = new List<string>();
    public List<List<string>> playerList = new List<List<string>>();

    public int currentRound;
    public int currentPageRound;
    [SerializeField] public Button backButton;

    [SerializeField] public TextMeshProUGUI tourneyTitleText;
    [SerializeField] public TextMeshProUGUI scenarioText;
    [SerializeField] public TextMeshProUGUI roundText;

    [SerializeField] public GameObject pairingsContainer;
    [SerializeField] public GameObject pairingsTextPrefab;
    [SerializeField] public GameObject gamesContainerPrefab;
    [SerializeField] public GameObject spacerPrefab;

    public void StartTourney()
    {
        this.tourney = CreateTourney();

        this.currentRound = 1;
        this.currentPageRound = 1;

        backButton.interactable = false;
        tourneyTitleText.text = tourneyName;
        scenarioText.text = scenarioList[currentPageRound-1];
        roundText.text = "Round " + currentPageRound;

        CreateContainers();
        Round startingRound = tourney.CreateRound(true, currentRound, scenarioList[currentRound-1]);
        tourney.roundList.Add(startingRound);
        FillContainers(startingRound);

        while(currentRound <= numberOfRounds)
        {
            break;
        }
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
            else if (dropdownName == "DropdownGames") numberOfGames = int.Parse(dropdown.options[dropdown.value].text);
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
            if(counter == 2)
            {
                counter = 0;
                playerCounter++;
                string parentName = inputField.transform.parent.gameObject.name;
                if (parentName.Contains("Good")) player.Add("Good");
                else if (parentName.Contains("Evil")) player.Add("Evil");
                playerList.Add(player);
            }
        }

        return new Tourney(tourneyName, numberOfRounds, scenarioList, numberOfGames, numberOfPlayers, playerList);
    }

    private void CreateContainers()
    {
        GameObject pairingsContainer = GameObject.Find("PairingsContainer");
        for (int i = 0; i < playerList.Count/2; i++)
        {
            Instantiate(pairingsTextPrefab, pairingsContainer.transform);
            for(int j = 0; j < numberOfGames; j++)
            {
                Instantiate(gamesContainerPrefab, pairingsContainer.transform);
                Instantiate(spacerPrefab, pairingsContainer.transform);
            }
        }
    }

    public void FillContainers(Round round)
    {
        List<Player> players = GetPlayers(round);
        List<GameObject> pairingsTexts = GetContainers("PairingsText(Clone)");
        List<GameObject> gamesContainers = GetContainers("GamesContainer(Clone)");

        int currentPlayerIndex = 0;
        int currentGameNumber = 1;

        // Insert data into PairingsText instances
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

        // Insert data into GamesContainer instances
        foreach (GameObject gamesContainer in gamesContainers)
        {
            // Insert game number into GamesText
            string gameTextString = "Game " + currentGameNumber;
            gamesContainer.transform.Find("GamesText").GetComponent<TextMeshProUGUI>().text = gameTextString;

            // Insert player names
            gamesContainer.transform.Find("GoodPlayer").GetComponent<TextMeshProUGUI>().text = players[currentPlayerIndex].name;
            gamesContainer.transform.Find("EvilPlayer").GetComponent<TextMeshProUGUI>().text = players[currentPlayerIndex + 1].name;

            // Insert points data
            if(round.gameList[currentGameNumber - 1].gamePoints.Count > 1)
            {
                Points points = round.gameList[currentGameNumber - 1].gamePoints[currentGameNumber - 1];
                gamesContainer.transform.Find("GainedGoodVPInput").GetComponent<TMP_InputField>().text = points.goodGainedVP.ToString();
                gamesContainer.transform.Find("LostGoodVPInput").GetComponent<TMP_InputField>().text = points.goodLostVP.ToString();
                gamesContainer.transform.Find("GainedEvilVPInput").GetComponent<TMP_InputField>().text = points.evilGainedVP.ToString();
                gamesContainer.transform.Find("LostEvilVPInput").GetComponent<TMP_InputField>().text = points.evilLostVP.ToString();

                // Insert leader killed data
                TMP_Dropdown leaderKilledGoodDropdown = gamesContainer.transform.Find("LeaderKilledGoodDropdown").GetComponent<TMP_Dropdown>();
                TMP_Dropdown leaderKilledEvilDropdown = gamesContainer.transform.Find("LeaderKilledEvilDropdown").GetComponent<TMP_Dropdown>();

                leaderKilledGoodDropdown.value = points.goodHasKilledLeader ? 1 : 0;
                leaderKilledEvilDropdown.value = points.evilHasKilledLeader ? 1 : 0;
            }

            

            currentGameNumber++;
            if (currentGameNumber > numberOfGames)
            {
                currentGameNumber = 1;
                currentPlayerIndex += 2;
                if (currentPlayerIndex >= numberOfPlayers)
                    currentPlayerIndex = 0;
            }
        }
    }

    private List<Player> GetPlayers(Round round)
    {
        List<Player> players = new List<Player>();

        for (int i = 0; i < numberOfPlayers/2; i++)
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
            if (child.name.Equals(containerName))
            {
                containers.Add(child.gameObject);
            }
        }

        return containers;
    }

    public bool AreAllInputsFilled()
    {
        bool result = true;

        foreach(TMP_InputField input in pairingsContainer.GetComponentsInChildren<TMP_InputField>())
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
        if(AreAllInputsFilled())
        {
            this.currentRound++; // ojo cuidao
            if (currentPageRound + 1 <= numberOfRounds)
            {
                this.currentPageRound++;
                backButton.interactable = true;
                scenarioText.text = scenarioList[currentPageRound - 1];
                roundText.text = "Round " + currentPageRound;
                ClearAllInputs();
            }
        }
    }

    public void PreviousRound()
    {
        if (currentPageRound - 1 >= 1)
        {
            this.currentPageRound--;
            scenarioText.text = scenarioList[currentPageRound-1];
            roundText.text = "Round " + currentPageRound;
        }
        if (currentPageRound == 1) backButton.interactable = false;
    }
}
