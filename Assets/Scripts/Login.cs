using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class Login : MonoBehaviour
{
    [SerializeField] private string loginEndpoint = "http://127.0.0.1:13756/account/login";
    [SerializeField] private string createEndpoint = "http://127.0.0.1:13756/account/create";

    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    public void OnLoginClick()
    {
        alertText.text = "Signing in...";
        ActivateButtons(false);

        StartCoroutine(TryLogin());
    }

    public void OnRegisterClick()
    {
        alertText.text = "Creating a new account...";
        ActivateButtons(false);

        StartCoroutine(TryCreate());
    }

    private IEnumerator TryLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if(username.Length < 3 || username.Length > 24)
        {
            alertText.text = "Invalid username";
            loginButton.interactable = true;
            yield break;
        }

        if (password.Length < 3 || password.Length > 24)
        {
            alertText.text = "Invalid password";
            loginButton.interactable = true;
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
            if(!request.downloadHandler.text.Equals("Invalid Credentials")) // login success
            {
                ActivateButtons(false);
                Account returnedAccount = JsonUtility.FromJson<Account>(request.downloadHandler.text); // retrieve data
                alertText.text = "Welcome" + returnedAccount.username;
            }
            else
            {
                alertText.text = "Invalid Credentials";
                ActivateButtons(true);
            }

            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            ActivateButtons(true);
            alertText.text = "Error connecting to the server...";
        }

        yield return null;
    }

    private IEnumerator TryCreate()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length < 3 || username.Length > 24)
        {
            alertText.text = "Invalid username";
            ActivateButtons(true);
            yield break;
        }

        if (password.Length < 3 || password.Length > 24)
        {
            alertText.text = "Invalid password";
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
            if (!request.downloadHandler.text.Equals("Invalid Credentials") && !request.downloadHandler.text.Equals("Username is already taken")) // login success
            {
                Account returnedAccount = JsonUtility.FromJson<Account>(request.downloadHandler.text); // retrieve data
                alertText.text = "Account has been created";
            }
            else alertText.text = "Username is already taken";

            Debug.Log(request.downloadHandler.text);
        }
        else alertText.text = "Error connecting to the server...";

        ActivateButtons(true);

        yield return null;
    }

    private void ActivateButtons(bool toggle)
    {
        loginButton.interactable = toggle;
        registerButton.interactable = toggle;
    } 

}
