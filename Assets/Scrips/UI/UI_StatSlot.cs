using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;
    [TextArea]
    [SerializeField] private string statDescription;
    private UI ui => GetComponentInParent<UI>();
    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }
    private void Start()
    {
        UpdataStatValueUI();
    }
    public void UpdataStatValueUI()
    {
        PlayerStat playerStats = PlayerManager.instance.player.GetComponent<PlayerStat>();
        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statsTip.ShowStatTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statsTip.HideStatTip();
    }
}
