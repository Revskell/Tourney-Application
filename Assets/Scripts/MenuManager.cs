using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;
    public static MenuManager Instance => instance;

    public string username;
    [SerializeField] private bool testingBool;
    [SerializeField] private TextMeshProUGUI welcomeText;

    [SerializeField] public GameObject logInMenuContainer;
    [SerializeField] public GameObject mainMenuContainer;
    [SerializeField] public GameObject createTourneyContainer;
    [SerializeField] public GameObject tourneyManagerContainer;
    [SerializeField] public GameObject listTourneyContainer;
    [SerializeField] public GameObject tourneyResultsContainer;

    [SerializeField] public Login login;
    [SerializeField] public TourneyRequest tourneyRequest;
    [SerializeField] public TourneyResultsHandler tourneyResults;
    [SerializeField] public TourneyListManager tourneyListManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        if(!testingBool) ShowLogInMenu();
        DontDestroyOnLoad(this.gameObject);
    }

    public void ShowMainMenu(string username)
    {
        mainMenuContainer.SetActive(true);
        logInMenuContainer.SetActive(false);
        createTourneyContainer.SetActive(false);
        tourneyManagerContainer.SetActive(false);
        listTourneyContainer.SetActive(false);
        tourneyResultsContainer.SetActive(false);

        if (this.username != null) this.username = username;
        else this.username = "";

        welcomeText.text = "Welcome " + username;
    }

    public void ShowLogInMenu()
    {
        logInMenuContainer.SetActive(true);
        mainMenuContainer.SetActive(false);
        createTourneyContainer.SetActive(false);
        tourneyManagerContainer.SetActive(false);
        listTourneyContainer.SetActive(false);
        tourneyResultsContainer.SetActive(false);

        login.RestartMenu();
    }

    public void ShowCreateTourneyMenu()
    {
        createTourneyContainer.SetActive(true);
        logInMenuContainer.SetActive(false);
        mainMenuContainer.SetActive(false);
        tourneyManagerContainer.SetActive(false);
        listTourneyContainer.SetActive(false);
        tourneyResultsContainer.SetActive(false);
    }

    public void ShowListTourneyMenu()
    {
        listTourneyContainer.SetActive(true);
        logInMenuContainer.SetActive(false);
        mainMenuContainer.SetActive(false);
        createTourneyContainer.SetActive(false);
        tourneyManagerContainer.SetActive(false);
        tourneyResultsContainer.SetActive(false);

        tourneyListManager.ShowTourneyList();
    }

    public void ShowTourneyManagerMenu()
    {
        tourneyManagerContainer.SetActive(true);
        logInMenuContainer.SetActive(false);
        mainMenuContainer.SetActive(false);
        createTourneyContainer.SetActive(false);
        listTourneyContainer.SetActive(false);
        tourneyResultsContainer.SetActive(false);
    }

    public void ShowTourneyResultsMenu(Tourney tourney, bool creatingTourney)
    {
        tourneyResultsContainer.SetActive(true);
        mainMenuContainer.SetActive(false);
        logInMenuContainer.SetActive(false);
        createTourneyContainer.SetActive(false);
        tourneyManagerContainer.SetActive(false);
        listTourneyContainer.SetActive(false);

        if (creatingTourney) StartCoroutine(tourneyRequest.TryCreateTourney(tourney));

        tourneyResults.ShowResults(tourney);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
