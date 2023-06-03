using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListMenuHandler : MonoBehaviour
{
    [SerializeField] public GameObject goodPlayerPrefab;
    [SerializeField] public GameObject evilPlayerPrefab;
    [SerializeField] public Transform container;
    [SerializeField] public TMP_Dropdown dropdown;

    private void Awake()
    {
        OnDropdownValueChanged();
    }

    public void OnDropdownValueChanged()
    {
        int selectedOption = int.Parse(dropdown.options[dropdown.value].text);

        for (int i = container.childCount - 1; i >= 0; i--) Destroy(container.GetChild(i).gameObject);

        for (int i = 0; i < selectedOption; i++)
        {
            GameObject newPlayer = null;

            if(i < selectedOption/2) newPlayer = Instantiate(goodPlayerPrefab, container);
            else if (i >= selectedOption/2 && i < selectedOption) newPlayer = Instantiate(evilPlayerPrefab, container);

            newPlayer.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Player " + (i + 1);
        }
    }
}
