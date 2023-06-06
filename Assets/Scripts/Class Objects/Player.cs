using System.Collections.Generic;

public class Player
{
    public string name { get; set; }
    public string nickname { get; set; }
    public string side { get; set; }

    public int totalGainedVP { get; set; }
    public int totalLostVP { get; set; }
    public int leadersKilled { get; set; }
    public int totalVP { get; set; }

    public Player(string name, string nickname, string side)
    {
        this.name = name;
        this.nickname = nickname;
        this.side = side;
        this.totalGainedVP = 0;
        this.totalLostVP = 0;
        this.leadersKilled = 0;
        this.totalVP = 0;
    }
    public Player(Player other)
    {
        this.name = other.name;
        this.nickname = other.nickname;
        this.side = other.side;
        this.totalGainedVP = other.totalGainedVP;
        this.totalLostVP = other.totalLostVP;
        this.leadersKilled = other.leadersKilled;
        this.totalVP = other.totalVP;
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
