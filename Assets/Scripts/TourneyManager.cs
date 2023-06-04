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
                if (string.IsNullOrEmpty(nickName)) name = "Nickname " + playerCounter;
                player.Add(nickName);
                player.Add(inputField.text);
                counter++;
            }
            if(counter >= 2)
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

    public void FillContainers()
    {

    }

    public void NextRound()
    {
        this.currentRound++; // ojo cuidao
        if (currentPageRound + 1 <= numberOfRounds)
        {
            this.currentPageRound++;
            backButton.interactable = true;
            scenarioText.text = scenarioList[currentPageRound-1];
            roundText.text = "Round " + currentPageRound;
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
