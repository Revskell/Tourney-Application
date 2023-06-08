using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TourneyContainer : MonoBehaviour
{
    public int tourneyIndex;

    [SerializeField] public TourneyListManager tourneyListManager;

    private void Awake()
    {
        this.tourneyListManager = MenuManager.Instance.tourneyListManager;

        if (tourneyListManager == null) Debug.Log("no");

        Button button = transform.GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            tourneyListManager.ShowResults(tourneyIndex);
        });
    }

    public void SetTourneyIndex(int index)
    {
        tourneyIndex = index;
    }

    public void OnTourneyButtonClick()
    {
        tourneyListManager.ShowResults(tourneyIndex);
    }
}
