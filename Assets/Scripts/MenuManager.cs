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
    [SerializeField] public GameObject joinTourneyContainer;
    [SerializeField] public GameObject listTourneyContainer;

    [SerializeField] public Login login;

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
        joinTourneyContainer.SetActive(false);
        listTourneyContainer.SetActive(false);

        if (this.username != null) this.username = username;

        welcomeText.text = "Welcome " + username;
    }

    public void ShowLogInMenu()
    {
        logInMenuContainer.SetActive(true);
        mainMenuContainer.SetActive(false);
        createTourneyContainer.SetActive(false);
        tourneyManagerContainer.SetActive(false);
        joinTourneyContainer.SetActive(false);
        listTourneyContainer.SetActive(false);

        login.RestartMenu();
    }

    public void ShowCreateTourneyMenu()
    {
        createTourneyContainer.SetActive(true);
        logInMenuContainer.SetActive(false);
        mainMenuContainer.SetActive(false);
        tourneyManagerContainer.SetActive(false);
        joinTourneyContainer.SetActive(false);
        listTourneyContainer.SetActive(false);
    }

    public void ShowJoinTourneyMenu()
    {
        joinTourneyContainer.SetActive(true);
        logInMenuContainer.SetActive(false);
        mainMenuContainer.SetActive(false);
        createTourneyContainer.SetActive(false);
        tourneyManagerContainer.SetActive(false);
        listTourneyContainer.SetActive(false);
    }

    public void ShowListTourneyMenu()
    {
        listTourneyContainer.SetActive(true);
        logInMenuContainer.SetActive(false);
        mainMenuContainer.SetActive(false);
        createTourneyContainer.SetActive(false);
        tourneyManagerContainer.SetActive(false);
        joinTourneyContainer.SetActive(false);
    }

    public void ShowTourneyManagerMenu()
    {
        tourneyManagerContainer.SetActive(true);
        logInMenuContainer.SetActive(false);
        mainMenuContainer.SetActive(false);
        createTourneyContainer.SetActive(false);
        joinTourneyContainer.SetActive(false);
        listTourneyContainer.SetActive(false);
    }
}
