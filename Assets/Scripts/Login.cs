using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private string authenticationEndpoint = "http://127.0.0.1:13756/account";

    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;

    public void OnLoginClick()
    {
        alertText.text = "Signing in...";
        loginButton.interactable = false;

        StartCoroutine(TryLogin());
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

        UnityWebRequest request = UnityWebRequest.Get($"{authenticationEndpoint}?rUsername={username}&rPassword={password}");
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
                alertText.text = "Welcome";
                loginButton.interactable = false;
            }
            else
            {
                alertText.text = "Invalid Credentials";
                loginButton.interactable = true;
            }

            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            loginButton.interactable = true;
            alertText.text = "Error connecting to the server...";
        }

        yield return null;
    }

}
