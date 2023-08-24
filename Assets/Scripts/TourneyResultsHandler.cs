using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TourneyResultsHandler : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI tourneyNameText;

    [SerializeField] public GameObject playersContainer;
    [SerializeField] public GameObject scenarioContainer;
    [SerializeField] public GameObject tourneyStatsContainer;

    [SerializeField] public GameObject playerPrefab;

    [SerializeField] public Button mainMenuButton;

    public Tourney tourney;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            MenuManager.Instance.ShowMainMenu(MenuManager.Instance.username);
        });
    }

    public void ShowResults(Tourney tourney)
    {
        this.tourney = tourney;

        tourneyNameText.text = tourney.tourneyName;

        Clear();
        FillScenarios();
        FillRounds();
        CreatePlayerContainers();
    }

    private void Clear()
    {
        for (int i = 0; i < scenarioContainer.transform.childCount; i++)
        {
            scenarioContainer.transform.Find("Scenario" + (i + 1) + "Text").GetComponent<TextMeshProUGUI>().text = "Scenario " + (i + 1) + ": ";
        }
        for (int i = playersContainer.transform.childCount - 1; i >= 0; i--) Destroy(playersContainer.transform.GetChild(i).gameObject);
    }

    private void FillScenarios()
    {
        for(int i = 0; i < 6; i++)
        {
            if(i < tourney.scenarioList.Count) scenarioContainer.transform.Find("Scenario" + (i+1) + "Text").GetComponent<TextMeshProUGUI>().text += tourney.scenarioList[i];
            else scenarioContainer.transform.Find("Scenario" + (i+1) + "Text").GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    private void FillRounds()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i >= tourney.nRounds) tourneyStatsContainer.transform.Find("Round" + (i + 1) + "Text").GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public void CreatePlayerContainers()
    {
        List<Player> rankedPlayerList = new List<Player>();

        foreach (Player player in tourney.rankedPlayerList) rankedPlayerList.Add(player);

        rankedPlayerList.Sort((p1, p2) => p2.totalVP.CompareTo(p1.totalVP));

        foreach (Player player in rankedPlayerList)
        {
            GameObject playerContainer = Instantiate(playerPrefab, playersContainer.transform);

            TextMeshProUGUI playerText = playerContainer.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>();

            if (player.side == "Good") playerText.color = Color.white;
            else if(player.side == "Evil") playerText.color = Color.black;

            playerText.text = player.name;

            int roundCounter = 1;
            int totalVPRound = 0;
            foreach(Round round in tourney.roundList)
            {
                totalVPRound = tourney.GetTotalPlayerRoundVP(player, roundCounter-1);
                playerContainer.transform.Find("Round" + roundCounter + "VPText").GetComponent<TextMeshProUGUI>().text = totalVPRound.ToString();
                roundCounter++;
            }

            playerContainer.transform.Find("TotalVPGainedPlayerText").GetComponent<TextMeshProUGUI>().text = player.totalGainedVP.ToString();
            playerContainer.transform.Find("TotalVPLostPlayerText").GetComponent<TextMeshProUGUI>().text = player.totalLostVP.ToString();
            playerContainer.transform.Find("TotalVPDifferencePlayerText").GetComponent<TextMeshProUGUI>().text = (player.totalGainedVP - player.totalLostVP).ToString();
            playerContainer.transform.Find("LeadersKilledPlayerText").GetComponent<TextMeshProUGUI>().text = player.leadersKilled.ToString();
            playerContainer.transform.Find("TotalVPPlayerText").GetComponent<TextMeshProUGUI>().text = player.totalVP.ToString();
        }
    }
}
