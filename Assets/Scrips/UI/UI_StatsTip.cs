using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatsTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ItemDescriptionText;

    public void ShowStatTip(string Description)
    {
        ItemDescriptionText.text = Description;
        gameObject.SetActive(true);
    }
    public void HideStatTip() => gameObject.SetActive(false);
}
