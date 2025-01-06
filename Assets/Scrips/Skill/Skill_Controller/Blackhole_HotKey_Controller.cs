using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;
    private Transform myEnemy;
    private Blackhole_Skill_Controller myBlackHole;

    private void Awake()
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();
    }
    public void SetUpHotKey(KeyCode myHotKey, Transform myEnemy, Blackhole_Skill_Controller myBlackHole)
    {
        this.myHotKey = myHotKey;
        this.myEnemy = myEnemy;
        this.myBlackHole = myBlackHole;
        this.myText.text = myHotKey.ToString();
    }


    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            myBlackHole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
        }
    }
}
