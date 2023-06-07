using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

public class TourneyRequest : MonoBehaviour
{
    [SerializeField] private string loginEndpoint = "http://127.0.0.1:13756/tourney/get";
    [SerializeField] private string createEndpoint = "http://127.0.0.1:13756/tourney/create";


    public IEnumerator TryCreateTourney(Tourney tourney)
    {
        string json = JsonConvert.SerializeObject(tourney);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(createEndpoint, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            CreateTourneyResponse response = JsonUtility.FromJson<CreateTourneyResponse>(request.downloadHandler.text);

            if (response.code == 0) // create success
            {
                Debug.Log("Tourney created successfully");
            }
            else
            {
                Debug.Log(response.msg);
            }
        }
        else
        {
            Debug.Log("shit's fucked");
        }

        request.Dispose();

        yield return null;
    }

    public IEnumerator TryGetTourney(string username)
    {

        yield return null;
    }

    private void FillRoundsAndPlayers(WWWForm form, List<Round> roundList, List<Player> rankedPlayerList)
    {
        // Add rRankedPlayerList
        for (int i = 0; i < rankedPlayerList.Count; i++)
        {
            Player player = rankedPlayerList[i];
            string playerNameKey = string.Format("rRankedPlayerList[{0}].name", i);
            string playerNicknameKey = string.Format("rRankedPlayerList[{0}].nickname", i);
            string playerSideKey = string.Format("rRankedPlayerList[{0}].side", i);
            string playerTotalGainedVPKey = string.Format("rRankedPlayerList[{0}].totalGainedVP", i);
            string playerTotalLostVPKey = string.Format("rRankedPlayerList[{0}].totalLostVP", i);
            string playerLeadersKilledKey = string.Format("rRankedPlayerList[{0}].leadersKilled", i);
            string playerTotalVPKey = string.Format("rRankedPlayerList[{0}].totalVP", i);

            form.AddField(playerNameKey, player.name);
            form.AddField(playerNicknameKey, player.nickname);
            form.AddField(playerSideKey, player.side);
            form.AddField(playerTotalGainedVPKey, player.totalGainedVP.ToString());
            form.AddField(playerTotalLostVPKey, player.totalLostVP.ToString());
            form.AddField(playerLeadersKilledKey, player.leadersKilled.ToString());
            form.AddField(playerTotalVPKey, player.totalVP.ToString());
        }

        // Add rRoundList
        for (int i = 0; i < roundList.Count; i++)
        {
            Round round = roundList[i];
            string roundNumberKey = string.Format("rRoundList[{0}].roundNumber", i);
            string roundScenarioKey = string.Format("rRoundList[{0}].roundScenario", i);

            form.AddField(roundNumberKey, round.roundNumber.ToString());
            form.AddField(roundScenarioKey, round.roundScenario);

            // Add gameList within the round
            for (int j = 0; j < round.gameList.Count; j++)
            {
                Game game = round.gameList[j];
                string gameGoodPlayerNameKey = string.Format("rRoundList[{0}].gameList[{1}].goodPlayer.name", i, j);
                string gameGoodPlayerNicknameKey = string.Format("rRoundList[{0}].gameList[{1}].goodPlayer.nickname", i, j);
                string gameGoodPlayerSideKey = string.Format("rRoundList[{0}].gameList[{1}].goodPlayer.side", i, j);
                string gameEvilPlayerNameKey = string.Format("rRoundList[{0}].gameList[{1}].evilPlayer.name", i, j);
                string gameEvilPlayerNicknameKey = string.Format("rRoundList[{0}].gameList[{1}].evilPlayer.nickname", i, j);
                string gameEvilPlayerSideKey = string.Format("rRoundList[{0}].gameList[{1}].evilPlayer.side", i, j);
                string gameGoodGainedVPKey = string.Format("rRoundList[{0}].gameList[{1}].gamePoints.goodGainedVP", i, j);
                string gameGoodLostVPKey = string.Format("rRoundList[{0}].gameList[{1}].gamePoints.goodLostVP", i, j);
                string gameEvilGainedVPKey = string.Format("rRoundList[{0}].gameList[{1}].gamePoints.evilGainedVP", i, j);
                string gameEvilLostVPKey = string.Format("rRoundList[{0}].gameList[{1}].gamePoints.evilLostVP", i, j);
                string gameGoodHasKilledLeaderKey = string.Format("rRoundList[{0}].gameList[{1}].gamePoints.goodHasKilledLeader", i, j);
                string gameEvilHasKilledLeaderKey = string.Format("rRoundList[{0}].gameList[{1}].gamePoints.evilHasKilledLeader", i, j);

                form.AddField(gameGoodPlayerNameKey, game.goodPlayer.name);
                form.AddField(gameGoodPlayerNicknameKey, game.goodPlayer.nickname);
                form.AddField(gameGoodPlayerSideKey, game.goodPlayer.side);
                form.AddField(gameEvilPlayerNameKey, game.evilPlayer.name);
                form.AddField(gameEvilPlayerNicknameKey, game.evilPlayer.nickname);
                form.AddField(gameEvilPlayerSideKey, game.evilPlayer.side);
                form.AddField(gameGoodGainedVPKey, game.gamePoints.goodGainedVP.ToString());
                form.AddField(gameGoodLostVPKey, game.gamePoints.goodLostVP.ToString());
                form.AddField(gameEvilGainedVPKey, game.gamePoints.evilGainedVP.ToString());
                form.AddField(gameEvilLostVPKey, game.gamePoints.evilLostVP.ToString());
                form.AddField(gameGoodHasKilledLeaderKey, game.gamePoints.goodHasKilledLeader.ToString());
                form.AddField(gameEvilHasKilledLeaderKey, game.gamePoints.evilHasKilledLeader.ToString());
            }
        }
    }
}
