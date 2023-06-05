using System.Collections.Generic;

public class Round
{
    public int roundNumber;
    public string roundScenario;
    public List<Game> gameList;

    public Round(int roundNumber, string roundScenario)
    {
        this.roundNumber = roundNumber;
        this.roundScenario = roundScenario;
        gameList = new List<Game>();
    }
}
