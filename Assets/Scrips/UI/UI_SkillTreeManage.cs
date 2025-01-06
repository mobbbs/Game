using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTreeManage : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI skillDescription;
    [SerializeField] public TextMeshProUGUI skillPointValue;
    private void Start()
    {
        skillDescription.text = "�㻹û��ѡ���ܣ� ��զ������͡�";
        UpdateSkillPoint();
    }
    public void UpdateSkillPoint()
    {
        skillPointValue.text = SkillManger.instance.GetSkillPoint().ToString();
    }
    
}
