using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class LoginTests
{
    private bool sceneLoaded = false;

    [OneTimeSetUp]
    public void LoadSceneOnce()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene("Login");
    }

    [UnityTest]
    public IEnumerator CreateAccountTest()
    {
        // We try to create a new Account
        Login login = Object.FindObjectOfType<Login>();

        Assert.NotNull(login, "Login component not found in the scene.");

        login.InputUsername("TestUser");
        login.InputPassword("TestPassword123");

        login.OnRegisterClick();

        yield return new WaitForSeconds(3.0f);

        string alertText = login.GetAlertText();
        Assert.That(alertText, Is.EqualTo("Account has been created").Or.EqualTo("Username is already taken"));

        yield return null;
    }

    [UnityTest]
    public IEnumerator LogInTest()
    {
        yield return new WaitUntil(() => sceneLoaded);

        // We try to log in with the account we've just created
        Login login = Object.FindObjectOfType<Login>();

        Assert.NotNull(login, "Login component not found in the scene.");

        login.InputUsername("TestUser");
        login.InputPassword("TestPassword123");

        login.OnLoginClick();

        yield return new WaitForSeconds(3.0f);

        string alertText = login.GetAlertText();
        Assert.AreEqual("Successfully Signed In", alertText);

        sceneLoaded = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoaded = true;
    }
}
