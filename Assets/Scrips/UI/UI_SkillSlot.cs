using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] public bool isLearn;
    [SerializeField] private List<GameObject> skillParent = new List<GameObject>();
    [SerializeField] private List<GameObject> skillBrother = new List<GameObject>();
    [SerializeField] private List<GameObject> skillChildren = new List<GameObject>();
    [SerializeField] private Image skillImage;
    [SerializeField] private Color LockedColor;
    [SerializeField] private int unlockedCost = 1;
    [SerializeField] private UI_SkillTreeManage skillTreeManage => GetComponentInParent<UI_SkillTreeManage>();
    private UI ui => GetComponentInParent<UI>();
    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = LockedColor;
        GetComponent<Button>().onClick.AddListener(() => UnlockedSkill());
    }


    private void UnlockedSkill()
    {
        if (!isLearn)
        {
            for (int i = 0; i < skillParent.Count; i++)
            {
                UI_SkillSlot parentSkillSlot = skillParent[i].GetComponent<UI_SkillSlot>();
                if (!parentSkillSlot.isLearn)
                {
                    return;
                }
            }
            for (int i = 0; i < skillBrother.Count; i++)
            {
                UI_SkillSlot brotherSkillSlot = skillBrother[i].GetComponent<UI_SkillSlot>();
                if (brotherSkillSlot.isLearn)
                {
                    return;
                }
            }
            isLearn = true;
            SkillManger.instance.ReduceSkilPoint(unlockedCost);
            skillTreeManage.UpdateSkillPoint();
            skillImage.color = Color.white;
        }
    }
    private void lockedSkill()
    {
        for (int i = 0; i < skillChildren.Count; i++)
        {
            UI_SkillSlot skillChildrenSlot = skillChildren[i].GetComponent<UI_SkillSlot>();
            if (skillChildrenSlot.isLearn)
            {
                return;
            }
        }
        isLearn = false;
        SkillManger.instance.IncreaseSkilPoint(unlockedCost);
        skillImage.color = LockedColor;
        skillTreeManage.UpdateSkillPoint();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTree.skillDescription.text = skillDescription;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("FUCK");
            lockedSkill();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            lockedSkill();
        }
    }
}
