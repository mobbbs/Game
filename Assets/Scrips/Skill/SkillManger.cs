using UnityEngine;

public class SkillManger : MonoBehaviour
{
    public static SkillManger instance;

    private int skillPoint = 99999;
    public Dash_Skill dash { get; private set; }
    public Clone_Skill clone { get; private set; }
    public Sword_Skill sword { get; private set; }
    public Blackhole_Skill blackhole { get; private set; }
    public Crystal_Skill crystal { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<Sword_Skill>();
        blackhole = GetComponent<Blackhole_Skill>();
        crystal = GetComponent<Crystal_Skill>();
    }

    public int GetSkillPoint() => skillPoint;
    public void ReduceSkilPoint(int _value) => skillPoint -= _value;
    public void IncreaseSkilPoint(int _value) => skillPoint += _value;
}
