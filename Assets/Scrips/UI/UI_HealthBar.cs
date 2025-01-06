using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private RectTransform myTransform;
    private Slider slider;
    private CharacterStats myStats;
    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        entity = GetComponentInParent<Entity>();
        myTransform = GetComponent<RectTransform>();
        myStats = GetComponentInParent<CharacterStats>();
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }
    private void Start()
    {
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHp();
        slider.value = Mathf.Max(0, myStats.currentHp);
    }
    private void Update()
    {
        UpdateHealthUI();
    }
    private void FlipUI() => myTransform.Rotate(0, 180, 0);
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
