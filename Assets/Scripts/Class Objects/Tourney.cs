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
    public int currentRound;

    public Tourney(string tourneyName, int rounds, List<string> scenarioList, int games, int players, List<List<string>> playerList)
    {
        this.tourneyCode = Random.Range(10000000, 99999999); // later check that it's not in use
        this.tourneyOwner = new Account(MenuManager.Instance.username);
        this.tourneyName = tourneyName;
        this.nRounds = rounds;
        this.nGames = games;
        this.nPlayers = players;
        this.scenarioList = scenarioList;
        this.playerList = PopulatePlayers(playerList);
        this.currentRound = 1;
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
}
