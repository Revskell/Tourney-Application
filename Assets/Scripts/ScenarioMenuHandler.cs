using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioMenuHandler : MonoBehaviour
{
    [SerializeField] public GameObject scenarioPrefab;
    [SerializeField] public Transform container;
    [SerializeField] public TMP_Dropdown dropdown;

    private void Awake()
    {
        OnDropdownValueChanged();
    }

    public void OnDropdownValueChanged()
    {
        int selectedOption = dropdown.value;

        for (int i = container.childCount - 1; i >= 0; i--) Destroy(transform.GetChild(i).gameObject);

        for(int i = 1; i <= selectedOption+1; i++)
        {
            GameObject newScenario = Instantiate(scenarioPrefab, container);
            newScenario.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Scenario " + i;
        }
    }
}
