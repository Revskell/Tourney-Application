using System.Collections.Generic;

public class Game 
{
    public Player goodPlayer;
    public Player evilPlayer;

    public Points gamePoints;

    public Game()
    {
    }

    public Game(Player goodPlayer, Player evilPlayer, Points gamePoints)
    {
        this.goodPlayer = goodPlayer;
        this.evilPlayer = evilPlayer;
        this.gamePoints = gamePoints;
    }
}
