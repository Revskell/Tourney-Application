[System.Serializable]
public class Account
{
    public string _id;
    public string username;

    public Account(string username)
    {
        this.username = username;
    }
}