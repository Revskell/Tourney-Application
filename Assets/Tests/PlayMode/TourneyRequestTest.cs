using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TourneyRequestTest
{
    [UnityTest]
    public IEnumerator CreateTourneyTest()
    {
        TourneyRequest tourneyRequest = Object.FindObjectOfType<TourneyRequest>();

        Assert.NotNull(tourneyRequest, "TourneyRequest component not found in the scene.");

        MenuManager.Instance.username = "TestUser";

        Tourney tourney = FillTourney();

        // We try to create the tourney
        MenuManager.Instance.ShowTourneyResultsMenu(tourney, true);

        yield return new WaitForSeconds(3.0f);

        Assert.That(tourneyRequest.hasBeenCreatedOrExists, Is.EqualTo(true));

        yield return null;
    }

    private Tourney FillTourney()
    {
        List<string> scenarioList = new List<string> { "Elimination", "Domination" };
        List<List<string>> playerList = new List<List<string>>
        {
            new List<string> {"TestPlayerGood", "TPG", "Good"},
            new List<string> {"TestPlayerEvil", "TPE", "Evil"}
        };

        Tourney tourney = new Tourney("TourneyTest", 2, scenarioList, 2, playerList);

        tourney.CreateRound(1, "Elimination");
        FillGamePoints(tourney, 1);
        tourney.RankPlayers(0);

        tourney.CreateRound(2, "Domination");
        FillGamePoints(tourney, 2);
        tourney.RankPlayers(1);

        return tourney;
    }

    private void FillGamePoints(Tourney tourney, int currentRound)
    {
        List<Points> points = new List<Points> { new Points(20, 0, false, 15, 5, false), new Points(25, 10, false, 20, 5, false) };

        int counter = 0;
        foreach (Game game in tourney.roundList[currentRound - 1].gameList)
        {
            game.gamePoints.goodGainedVP = points[counter].goodGainedVP;
            game.gamePoints.goodLostVP = points[counter].goodLostVP;
            game.gamePoints.goodHasKilledLeader = points[counter].goodHasKilledLeader;

            game.gamePoints.evilGainedVP = points[counter].evilGainedVP;
            game.gamePoints.evilLostVP = points[counter].evilLostVP;
            game.gamePoints.evilHasKilledLeader = points[counter].evilHasKilledLeader;

            counter++;
        }
    }

    [UnityTest]
    public IEnumerator GetTourneyTest()
    {
        TourneyRequest tourneyRequest = Object.FindObjectOfType<TourneyRequest>();

        Assert.NotNull(tourneyRequest, "TourneyRequest component not found in the scene.");

        MenuManager.Instance.username = "TestUser";

        // We try to get the tourney we've just created
        MenuManager.Instance.ShowListTourneyMenu();

        yield return new WaitForSeconds(3.0f);

        Assert.That(tourneyRequest.tourneyFound, Is.EqualTo(true));
    }
}
