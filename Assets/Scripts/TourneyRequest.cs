using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using Unity.Serialization.Json;

public class TourneyRequest : MonoBehaviour
{
    [SerializeField] private string getEndpoint = "http://127.0.0.1:13756/tourney/get";
    [SerializeField] private string createEndpoint = "http://127.0.0.1:13756/tourney/create";

    public List<Tourney> tourneyList;

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
            Debug.Log("Error in the server");
        }

        request.Dispose();

        yield return null;
    }

    public IEnumerator TryGetTourney(string username, System.Action callback)
    {
        string url = getEndpoint + "?rUsername=" + UnityWebRequest.EscapeURL(username);

        // Create the UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            // An error occurred
            Debug.Log("Error: " + request.error);
        }
        else
        {
            // Request successful
            string responseJson = request.downloadHandler.text;

            // Parse the response JSON
            ResponseData response = JsonUtility.FromJson<ResponseData>(responseJson);

            // Check the response code
            int responseCode = response.code;
            if (responseCode == 0)
            {
                // Tourney found
                List<TourneyData> tourneyDataList = response.data;
                this.tourneyList = new List<Tourney>();
                tourneyList.Clear();

                foreach (TourneyData tourneyData in tourneyDataList)
                {
                    Tourney tourney = ParseTourneyData(tourneyData);
                    tourneyList.Add(tourney);
                }
            }
            else
            {
                // No tourneys found
                Debug.Log("No tourneys found");
            }
        }

        callback?.Invoke();

        yield return null;
    }

    private Tourney ParseTourneyData(TourneyData tourneyData)
    {

        // Parse the tourney data and create a Tourney object
        Tourney tourney = new Tourney();
        tourney.tourneyOwner = tourneyData.tourneyOwner;
        tourney.tourneyName = tourneyData.tourneyName;
        tourney.nRounds = tourneyData.nRounds;
        tourney.nPlayers = tourneyData.nPlayers;
        tourney.rankedPlayerList = new List<Player>();
        tourney.roundList = new List<Round>();
        tourney.scenarioList = new List<string>();

        // Parse the rankedPlayerList
        foreach (PlayerData playerData in tourneyData.rankedPlayerList)
        {
            Player player = ParsePlayerData(playerData);

            tourney.rankedPlayerList.Add(player);
        }

        // Parse the roundList
        foreach (RoundData roundData in tourneyData.roundList)
        {
            Round round = new Round();
            round.roundNumber = roundData.roundNumber;
            round.roundScenario = roundData.roundScenario;
            round.gameList = new List<Game>();

            // Parse the gameList
            foreach (GameData gameData in roundData.gameList)
            {
                Game game = new Game();

                // Parse the goodPlayer
                PlayerData goodPlayerData = gameData.goodPlayer;
                Player goodPlayer = ParsePlayerData(goodPlayerData);
                game.goodPlayer = goodPlayer;

                // Parse the evilPlayer
                PlayerData evilPlayerData = gameData.evilPlayer;
                Player evilPlayer = ParsePlayerData(evilPlayerData);
                game.evilPlayer = evilPlayer;

                // Parse the gamePoints
                PointsData pointsData = gameData.gamePoints;
                Points points = ParsePointsData(pointsData);
                game.gamePoints = points;

                round.gameList.Add(game);
            }

            tourney.roundList.Add(round);
        }

        // Parse the scenarioList
        foreach (string scenario in tourneyData.scenarioList) tourney.scenarioList.Add(scenario);

        return tourney;
    }

    private Player ParsePlayerData(PlayerData playerData)
    {
        Player player = new Player();
        player.name = playerData.name;
        player.nickname = playerData.nickname;
        player.side = playerData.side;
        player.totalGainedVP = playerData.totalGainedVP;
        player.totalLostVP = playerData.totalLostVP;
        player.leadersKilled = playerData.leadersKilled;
        player.totalVP = playerData.totalVP;

        return player;
    }

    private Points ParsePointsData(PointsData pointsData)
    {
        Points points = new Points();
        points.goodGainedVP = pointsData.goodGainedVP;
        points.goodLostVP = pointsData.goodLostVP;
        points.evilGainedVP = pointsData.evilGainedVP;
        points.evilLostVP = pointsData.evilLostVP;
        points.goodHasKilledLeader = pointsData.goodHasKilledLeader;
        points.evilHasKilledLeader = pointsData.evilHasKilledLeader;

        return points;
    }

    [System.Serializable]
    public class TourneyData
    {
        public string tourneyOwner;
        public string tourneyName;
        public int nRounds;
        public int nPlayers;
        public List<PlayerData> rankedPlayerList;
        public List<RoundData> roundList;
        public List<string> scenarioList;
    }

    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public string nickname;
        public string side;
        public int totalGainedVP;
        public int totalLostVP;
        public int leadersKilled;
        public int totalVP;
    }

    [System.Serializable]
    public class RoundData
    {
        public int roundNumber;
        public string roundScenario;
        public List<GameData> gameList;
    }

    [System.Serializable]
    public class GameData
    {
        public PlayerData goodPlayer;
        public PlayerData evilPlayer;
        public PointsData gamePoints;
    }

    [System.Serializable]
    public class PointsData
    {
        public int goodGainedVP;
        public int goodLostVP;
        public int evilGainedVP;
        public int evilLostVP;
        public bool goodHasKilledLeader;
        public bool evilHasKilledLeader;
    }

    [System.Serializable]
    public class ResponseData
    {
        public int code;
        public string msg;
        public List<TourneyData> data;
    }
}

