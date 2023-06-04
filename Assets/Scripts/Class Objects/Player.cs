using System.Collections.Generic;

public class Player
{
    public string name;
    public string nickname;
    public string force;

    public Player(string name, string nickname, string force)
    {
        this.name = name;
        this.nickname = nickname;
        this.force = force;
    }

    public Player(List<string> player)
    {
        int counter = 0;
        foreach(string data in player)
        {
            if (counter == 0) this.name = data;
            else if (counter == 1) this.nickname = data;
            else if (counter == 2) this.force = data;
            counter++;
        }
    }
}
