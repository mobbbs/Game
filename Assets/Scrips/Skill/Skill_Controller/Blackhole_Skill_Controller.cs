using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    private List<Transform> enemyTarget = new List<Transform>();
    private List<GameObject> creatHotKey = new List<GameObject>();
    private float maxSize;
    private float growSpeed;
    private float blackholeTimer;
    private bool canGrow;
    private bool canShrink;
    private float shrinkSpeed;
    private bool canCreatHotKey;
    private bool playerCanDisapper;

    private bool cloneAttackReleased;
    private int amountOfAttack;
    private float cloneAttackCooldown;
    private float cloneAttackTimer;
    private bool crystalInsteadOfClone;
    public bool playerCanExitState;

    private void Start()
    {
        canCreatHotKey = true;
        canGrow = true;
        canShrink = false;
        playerCanExitState = false;
        playerCanDisapper = true;
    }
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer <= 0)
        {
            blackholeTimer = Mathf.Infinity;
            if (enemyTarget.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                cloneAttackFinish();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0)
            {
                BlackHoleAbilityFinish();
            }
        }
    }

    private void BlackHoleAbilityFinish()
    {
        Destroy(gameObject);
    }

    public void SetUpBlackhole(float maxSize, float growSpeed, float shrinkSpeed, int amountOfAttack, float cloneAttackCooldown, float blackholeDuration)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.amountOfAttack = amountOfAttack;
        this.cloneAttackCooldown = cloneAttackCooldown;
        blackholeTimer = blackholeDuration;
        crystalInsteadOfClone = SkillManger.instance.clone.crystalInsteadOfClone;
        if (crystalInsteadOfClone)
        {
            playerCanDisapper = false;
        }
    }
    private void ReleaseCloneAttack()
    {
        DestroyHotKey();
        cloneAttackReleased = true;
        canCreatHotKey = false;
        if (playerCanDisapper)
        {
            PlayerManager.instance.player.MakeTransparent(true);
            playerCanDisapper = false;
        }
    }
    private void CloneAttackLogic()
    {
        if (cloneAttackTimer <= 0 && cloneAttackReleased && amountOfAttack > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, enemyTarget.Count);

            float xoffset = (Random.Range(0, 100) > 50 ? 1.5f : -1.5f);

            if (crystalInsteadOfClone)
            {
                SkillManger.instance.crystal.CreateCrystal();
                SkillManger.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                if (enemyTarget.Count > 0)
                {
                    SkillManger.instance.clone.CreateClone(enemyTarget[randomIndex], new Vector3(xoffset, 0, 0));
                }
            }

            amountOfAttack--;
            if (amountOfAttack <= 0)
            {
                Invoke("cloneAttackFinish", 1f);
            }
        }
    }

    private void cloneAttackFinish()
    {
        DestroyHotKey();
        PlayerManager.instance.player.MakeTransparent(false);
        cloneAttackReleased = false;
        playerCanExitState = true;
        canShrink = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreatHotKey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }
    private void DestroyHotKey()
    {
        for (int i = 0; i < creatHotKey.Count; i++)
        {
            Destroy(creatHotKey[i]);
        }
    }
    private void CreatHotKey(Collider2D collision)
    {
        if (keyCodeList.Count == 0)
        {
            Debug.LogWarning("Not more keycode in the keycodeList");
            return;
        }

        if (!canCreatHotKey)
        {
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        KeyCode newKeyCode = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(newKeyCode);
        creatHotKey.Add(newHotKey);
        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();
        newHotKeyScript.SetUpHotKey(newKeyCode, collision.transform, this);
    }
    public void AddEnemyToList(Transform enemy) => enemyTarget.Add(enemy);
}
