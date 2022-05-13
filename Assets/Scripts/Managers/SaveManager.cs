using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public const string TOTAL_PLAYING_MATCHS = "Saves.TotalPlayingMatch";
    public const string MATCH_DATA_PREFS = "Saves.MatchData";

    private MatchDataSave _matchDataSave;
    public static SaveManager Instance { get; private set; }
    public MatchDataSave MatchDataSave { get => _matchDataSave; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);

            LoadMatchDatas();

            return;
        }

        Destroy(this.gameObject);
    }

    public void SaveMatchData(MatchData matchData)
    {
        MatchDataSave.MatchDatas.Add(matchData);

        print(JsonUtility.ToJson(MatchDataSave));

        PlayerPrefs.SetString(MATCH_DATA_PREFS, JsonUtility.ToJson(MatchDataSave));

        Debug.Log("<color=yellow>[SAVE_MANAGER]:</color> Match Data Saved");
    }

    public List<MatchData> LoadMatchDatas()
    {
        _matchDataSave = new MatchDataSave();

        if (PlayerPrefs.HasKey(MATCH_DATA_PREFS))
        {
            _matchDataSave = JsonUtility.FromJson<MatchDataSave>(PlayerPrefs.GetString(MATCH_DATA_PREFS));

            print(JsonUtility.ToJson(MatchDataSave));
            Debug.Log("<color=yellow>[SAVE_MANAGER]:</color> Match Data Loaded");
        }

        return MatchDataSave.MatchDatas;
    }

    public static void DeleteMatchData()
    {
        PlayerPrefs.DeleteKey(MATCH_DATA_PREFS);
        PlayerPrefs.DeleteKey(TOTAL_PLAYING_MATCHS);

        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {

    }
}

[System.Serializable]
public class MatchDataSave
{
    public List<MatchData> MatchDatas = new List<MatchData>();
}

[System.Serializable]
public struct MatchData
{
    public int index;
    public TableStatus.CommandType winnerCommandType;
    public string matchStartDateTime;
    public string matchEndDateTime;
    public string matchDuration;
}
