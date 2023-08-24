using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Login : MonoBehaviour
{
    [SerializeField] private string loginEndpoint = "http://127.0.0.1:13756/account/login";
    [SerializeField] private string createEndpoint = "http://127.0.0.1:13756/account/create";

    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{5,30})";

    public void OnLoginClick()
    {
        alertText.gameObject.SetActive(true);
        alertText.color = Color.white;
        alertText.text = "Signing in...";
        ActivateButtons(false);

        StartCoroutine(TryLogin());
    }

    public void OnRegisterClick()
    {
        alertText.gameObject.SetActive(true);
        alertText.color = Color.white;
        alertText.text = "Creating a new account...";
        ActivateButtons(false);

        StartCoroutine(TryCreate());
    }

    private IEnumerator TryLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if(username.Length < 3 || username.Length > 24)
        {
            alertText.color = Color.red;
            alertText.text = "Invalid username";
            ActivateButtons(true);
            yield break;
        }

        if (!Regex.IsMatch(password, PASSWORD_REGEX))
        {
            alertText.color = Color.red;
            alertText.text = "Invalid credentials";
            ActivateButtons(true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        UnityWebRequest request = UnityWebRequest.Post(loginEndpoint, form);
        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;
            if (startTime > 10.0f) break;
            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

            if(response.code == 0)
            {
                ActivateButtons(false);
                MenuManager.Instance.ShowMainMenu(response.data.username);
            }
            else
            {
                alertText.color = Color.red;
                alertText.text = "Invalid Credentials";
                ActivateButtons(true);
            }
        }
        else
        {
            ActivateButtons(true);
            alertText.color = Color.red;
            alertText.text = "Error connecting to the server...";
        }

        request.Dispose();

        yield return null;
    }

    private IEnumerator TryCreate()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username.Length < 3 || username.Length > 24)
        {
            alertText.color = Color.red;
            alertText.text = "Invalid username";
            ActivateButtons(true);
            yield break;
        }

        if(!Regex.IsMatch(password, PASSWORD_REGEX))
        {
            alertText.color = Color.red;
            alertText.text = "Password is unsafe";
            ActivateButtons(true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        UnityWebRequest request = UnityWebRequest.Post(createEndpoint, form);
        var handler = request.SendWebRequest();


        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;
            if (startTime > 10.0f) break;
            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            CreateResponse response = JsonUtility.FromJson<CreateResponse>(request.downloadHandler.text);

            if (response.code.Equals(0)) // login success
            {
                alertText.color = Color.white;
                alertText.text = "Account has been created";
            }
            else
            {
                alertText.color = Color.red;
                switch(response.code)
                {
                    case 1: alertText.text = "Invalid credentials"; break;
                    case 2: alertText.text = "Username is already taken"; break;
                    case 3: alertText.text = "Password is unsafe"; break;
                    default: alertText.text = "Error"; break;
                }
            }
        }
        else alertText.text = "Error connecting to the server...";

        ActivateButtons(true);
        request.Dispose();

        yield return null;
    }

    private void CheckUsernameAndPassword()
    {

    }

    private void ActivateButtons(bool toggle)
    {
        loginButton.interactable = toggle;
        registerButton.interactable = toggle;
    } 

    public void RestartMenu()
    {
        ActivateButtons(true);
        alertText.color = Color.white;
        alertText.text = null;
    }
}
