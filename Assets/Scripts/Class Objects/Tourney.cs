using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Tourney
{
    public string _id;
    public int tourneyCode;
    public Account tourneyOwner;
    public string tourneyName;
    public int nRounds;
    public int nGames;
    public int nPlayers;

    public List<Player> playerList;

    public List<Round> roundList;
    public List<string> scenarioList;

    public Tourney(string tourneyName, int rounds, List<string> scenarioList, int games, int players, List<List<string>> playerList)
    {
        this.tourneyCode = Random.Range(10000000, 99999999); // later check that it's not in use
        this.tourneyOwner = new Account(MenuManager.Instance.username);
        this.tourneyName = tourneyName;
        this.nRounds = rounds;
        this.nGames = games;
        this.nPlayers = players;
        this.roundList = new List<Round>();
        this.scenarioList = scenarioList;
        this.playerList = PopulatePlayers(playerList);
    }

    public List<Player> PopulatePlayers(List<List<string>> playerList)
    {
        List<Player> players = new List<Player>();

        foreach(List<string> player in playerList)
        {
           players.Add(new Player(player));
        }

        return players;
    }

    public Round CreateRound(bool firstRound, int roundNumber, string roundScenario)
    {
        Round round = new Round(roundNumber, roundScenario);

        if(firstRound)
        {
            List<Player> availablePlayers = new List<Player>(playerList);

            while (availablePlayers.Count >= 2)
            {
                Player goodPlayer = GetPlayerBySide(availablePlayers, "Good");
                Player evilPlayer = GetPlayerBySide(availablePlayers, "Evil");

                if (goodPlayer == null || evilPlayer == null) break;

                // Create a new game and add it to the round's gameList
                Game game = new Game(goodPlayer, evilPlayer, new List<Points>());
                round.gameList.Add(game);

                // Remove the paired players from the available players list
                availablePlayers.Remove(goodPlayer);
                availablePlayers.Remove(evilPlayer);
            }

            return round;
        }
        else
        {

        }

        return round;
    }

    private Player GetPlayerBySide(List<Player> players, string side)
    {
        foreach (Player player in players)
        {
            if (player.side == side) return player;
        }

        return null;
    }
}
