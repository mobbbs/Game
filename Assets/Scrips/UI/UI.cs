using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject Charactor;
    [SerializeField] GameObject SkillTree;
    [SerializeField] GameObject Craft;
    [SerializeField] GameObject Options;

    public UI_ItemToolTip itemToolTip => GetComponentInChildren<UI_ItemToolTip>(true);
    public UI_StatsTip statsTip => GetComponentInChildren<UI_StatsTip>(true);
    public UI_CraftWindowController craftWindow => GetComponentInChildren<UI_CraftWindowController>(true);
    public UI_SkillTreeManage skillTree => GetComponentInChildren<UI_SkillTreeManage>(true); 
    public void Start()
    {
        SwitchTo(null);
        itemToolTip.gameObject.SetActive(false);
        statsTip.gameObject.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SwitchWithKeyTo(Charactor);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SwitchWithKeyTo(SkillTree);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchWithKeyTo(Craft);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(Options);
        }
    }
    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if (menu != null)
        {
            menu.SetActive(true);
        }
    }

    public void SwitchWithKeyTo(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            return;
        }
        if (menu != null)
        {
            SwitchTo(menu);
        }
    }
}
