using System.Collections.Generic;
using UnityEngine;

public class MatchHistoryViewer : MonoBehaviour
{
    [SerializeField] private GameObject _matchHistoryPrefab;
    [SerializeField] private Transform _matchHistoryContentTransform;

    private List<MatchData> _matchDatas;

    private void OnEnable()
    {
        if (_matchHistoryContentTransform.childCount > 0)
        {
            for (int i = 0; i < _matchHistoryContentTransform.childCount; i++)
            {
                Destroy(_matchHistoryContentTransform.GetChild(i).gameObject);
            }
        }

        _matchDatas = SaveManager.Instance.LoadMatchDatas();

        foreach (MatchData matchData in _matchDatas)
        {
            GameObject MatchHistoryObject = Instantiate(_matchHistoryPrefab, _matchHistoryContentTransform);

            MatchHistoryObject.GetComponent<MatchHistory>().UpdatePanel(matchData);
        }
    }

    private void OnDestroy()
    {

    }
}
