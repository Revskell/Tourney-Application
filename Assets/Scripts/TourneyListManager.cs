using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TourneyListManager : MonoBehaviour
{

    [SerializeField] public GameObject tourneyPrefab;
    [SerializeField] public TextMeshProUGUI usernameText;
    [SerializeField] public GameObject tourneyContainer;
    [SerializeField] public TourneyRequest tourneyRequest;

    private List<Tourney> tourneyList;

    public void ShowTourneyList()
    {
        tourneyList = new List<Tourney>();
        tourneyList.Clear();

        for (int i = tourneyContainer.transform.childCount - 1; i >= 0; i--) Destroy(tourneyContainer.transform.GetChild(i).gameObject);

        this.usernameText.text = MenuManager.Instance.username;

        StartCoroutine(tourneyRequest.TryGetTourney(MenuManager.Instance.username, () =>
        {
            foreach (Tourney tourney in tourneyRequest.tourneyList)
            {
                tourneyList.Add(tourney);
            }

            CreateAndFillContainers();
        }
        ));
    }

    public void CreateAndFillContainers()
    {
        int counter = 0;
        foreach(Tourney tourney in tourneyList)
        {
            GameObject container = Instantiate(tourneyPrefab, tourneyContainer.transform);

            TourneyContainer tc = container.transform.Find("TourneyButton").GetComponent<TourneyContainer>();
            tc.SetTourneyIndex(counter);

            container.transform.Find("TourneyNameText").GetComponent<TextMeshProUGUI>().text = tourney.tourneyName;
            container.transform.Find("NRoundsText").GetComponent<TextMeshProUGUI>().text += tourney.nRounds.ToString();
            container.transform.Find("NPlayersText").GetComponent<TextMeshProUGUI>().text += tourney.nPlayers.ToString();

            counter++;
        }
    }

    public void ShowResults(int option)
    {
        Tourney tourney = tourneyList[option];
        MenuManager.Instance.ShowTourneyResultsMenu(tourney, false);
    }
}
