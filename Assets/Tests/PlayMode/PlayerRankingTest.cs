using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerRankingTest : MonoBehaviour
{
    private Tourney tourney;

    [UnityTest]
    public IEnumerator CheckRankingsTest()
    {
        tourney = FillTourney();

        tourney.CreateRound(1, "Elimination");
        FillGamePoints(1);
        tourney.RankPlayers(0);

        Assert.That(tourney.rankedPlayerList[0].name, Is.EqualTo("TestPlayerGood"));
        Assert.That(tourney.rankedPlayerList[0].totalVP, Is.EqualTo(3));
        Assert.That(tourney.rankedPlayerList[1].totalVP, Is.EqualTo(0));

        yield return null;
    }

    private Tourney FillTourney()
    {
        List<string> scenarioList = new List<string> { "Elimination" };
        List<List<string>> playerList = new List<List<string>>
            {
                new List<string> {"TestPlayerGood", "TPG", "Good"},
                new List<string> {"TestPlayerEvil", "TPE", "Evil"}
            };

        return new Tourney("TourneyTest", 1, scenarioList, 2, playerList);
    }

    private void FillGamePoints(int currentRound)
    {
        Points points = new Points(20, 0, false, 15, 5, false);

        tourney.roundList[currentRound - 1].gameList.Add(new Game(new Player("TestPlayerGood", "TPG", "Good"), new Player("TestPlayerEvil", "TPE", "Evil"), points));
    }
}
