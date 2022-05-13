using TMPro;
using UnityEngine;

public class MatchHistory : MonoBehaviour
{
    [SerializeField] private TMP_Text _indexTMP_Text;
    [SerializeField] private TMP_Text _winnerTMP_Text;
    [SerializeField] private TMP_Text _startMatchTMP_Text;
    [SerializeField] private TMP_Text _endMatchTMP_Text;
    [SerializeField] private TMP_Text _durationMatchTMP_Text;

    public void UpdatePanel(MatchData matchData)
    {
        _indexTMP_Text.text = matchData.index.ToString();

        string winnerCommand;

        switch (matchData.winnerCommandType)
        {
            case TableStatus.CommandType.None:
                winnerCommand = "НИЧЬЯ";
                break;
            case TableStatus.CommandType.Cross:
                winnerCommand = "КРЕСТИКИ";
                break;
            case TableStatus.CommandType.Zero:
                winnerCommand = "НОЛИКИ";
                break;
            default:
                winnerCommand = "НЕОПРЕДЕЛЕНО";
                break;
        }

        _winnerTMP_Text.text = winnerCommand;
        _startMatchTMP_Text.text = matchData.matchStartDateTime;
        _endMatchTMP_Text.text = matchData.matchEndDateTime;
        _durationMatchTMP_Text.text = matchData.matchDuration;
    }
}
