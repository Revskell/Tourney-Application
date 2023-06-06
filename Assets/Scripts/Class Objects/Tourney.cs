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
    public List<Player> rankedPlayerList;

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
        this.rankedPlayerList = new List<Player>();
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

    public void CreateRound(bool firstRound, int roundNumber, string roundScenario)
    {
        Round round = new Round(roundNumber, roundScenario);

        List<Player> availablePlayers = new List<Player>(playerList);
        if (!firstRound)
        {
            availablePlayers = RankPlayers(roundNumber);
            rankedPlayerList = new List<Player>();
            foreach (Player player in availablePlayers) rankedPlayerList.Add(player);
        }

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

        this.roundList.Add(round);
    }

    private List<Player> RankPlayers(int roundNumber)
    {
        List<Player> rankedPlayers = new List<Player>();

        foreach (Player player in playerList)
        {
            foreach (Game game in roundList[roundNumber - 2].gameList)
            {
                if (game.goodPlayer.name == player.name && game.goodPlayer.nickname == player.nickname)
                {
                    foreach (Points points in game.gamePoints)
                    {
                        player.totalGainedVP += points.goodGainedVP;
                        player.totalLostVP += points.goodLostVP;
                        if (points.goodHasKilledLeader) player.leadersKilled++;
                        if ((points.goodGainedVP - points.goodLostVP) > (points.evilGainedVP - points.evilLostVP)) player.totalVP += 3;
                        else if ((points.goodGainedVP - points.goodLostVP) == (points.evilGainedVP - points.evilLostVP)) player.totalVP++;
                    }
                }
                else if (game.evilPlayer.name == player.name && game.evilPlayer.nickname == player.nickname)
                {
                    foreach (Points points in game.gamePoints)
                    {
                        player.totalGainedVP += points.evilGainedVP;
                        player.totalLostVP += points.evilLostVP;
                        if (points.goodHasKilledLeader) player.leadersKilled++;
                        if ((points.evilGainedVP - points.evilLostVP) > (points.goodGainedVP - points.goodLostVP)) player.totalVP += 3;
                        else if ((points.goodGainedVP - points.goodLostVP) == (points.evilGainedVP - points.evilLostVP)) player.totalVP++;
                    }
                }
            }

            rankedPlayers.Add(player);
        }

        rankedPlayers.Sort((p1, p2) => p2.totalVP.CompareTo(p1.totalVP));

        return rankedPlayers;
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
