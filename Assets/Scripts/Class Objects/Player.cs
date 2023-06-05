using System.Collections.Generic;

public class Player
{
    public string name;
    public string nickname;
    public string side;

    public Player(string name, string nickname, string side)
    {
        this.name = name;
        this.nickname = nickname;
        this.side = side;
    }

    public Player(List<string> player)
    {
        int counter = 0;
        foreach(string data in player)
        {
            if (counter == 0) this.name = data;
            else if (counter == 1) this.nickname = data;
            else if (counter == 2) this.side = data;
            counter++;
        }
    }
}
