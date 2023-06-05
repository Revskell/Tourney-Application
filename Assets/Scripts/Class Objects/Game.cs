using System.Collections.Generic;

public class Game 
{
    public Player goodPlayer;
    public Player evilPlayer;

    public List<Points> gamePoints;

    public Player winner;

    public Game(Player goodPlayer, Player evilPlayer, List<Points> gamePoints)
    {
        this.goodPlayer = goodPlayer;
        this.evilPlayer = evilPlayer;
        this.gamePoints = gamePoints;
    }

    public void ResolveWinner()
    {
        if(gamePoints != null)
        {
            
        }
    }
}
