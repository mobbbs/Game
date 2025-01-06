using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Random = UnityEngine.Random;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    maxHp,
    armor,
    evasion,
    magicResistance,
    damage,
    critPower,
    cirtChance,
    fireDamage,
    iceDamage,
    lightningDamage
}
public class CharacterStats : MonoBehaviour
{

    private EntityFx fx;

    [SerializeField] private int facingDir;
    [Header("Major Stat")]
    public Stat strength;
    public Stat agility;
    public Stat intelligence;
    public Stat vitality;

    [Header("Defensive stat")]
    public Stat maxHp;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Offensive stat")]
    public Stat damage;
    public Stat critPower;
    public Stat cirtChance;

    [Header("Magic Stat")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;


    [SerializeField] private float ailmentsDuration = 4;
    public bool isIgnited; // dose damage over time
    public int ignitedDamage;
    public float ignitedTimer;
    public float ignitedDamgeTimer;
    public float ignitedDamgeCooldown;
    private ignitedIcon_Controller ignitedIcon;

    public bool isChilled; // reduce armor by 20%
    public float chilledTimer;
    private chilledIcon_Controller chilledIcon;

    public bool isShocked; // reduce accuracy by 20%, the shocked can cause Thunder
    public float shockedTimer;
    private shockedIcon_Controller shockedIcon;
    public int shockedDamage;
    public GameObject shockedPrefab;


    [SerializeField] public int currentHp;
    public bool isDead { get; private set; }
    public System.Action onHealthChanged;
    protected virtual void Awake()
    {
        fx = GetComponent<EntityFx>();
        ignitedIcon = GetComponentInChildren<ignitedIcon_Controller>();
        chilledIcon = GetComponentInChildren<chilledIcon_Controller>();
        shockedIcon = GetComponentInChildren<shockedIcon_Controller>();
    }
    protected virtual void Start()
    {
        currentHp = maxHp.GetValue();
        critPower.setBaseValue(150);
        isDead = false;
    }
    protected virtual void Update()
    {
        facingDir = GetComponent<Entity>().facingDir;
        if (isIgnited)
        {
            ignitedTimer -= Time.deltaTime;
            ignitedDamgeTimer -= Time.deltaTime;
            if (ignitedTimer < 0)
            {
                isIgnited = false;
                ignitedIcon.CloseIcon();
            }
            else if (ignitedDamgeTimer <= 0)
            {
                ignitedDamgeTimer = ignitedDamgeCooldown;
                currentHp -= ignitedDamage;
                DecreaseHealthBy(ignitedDamage);
            }
        }
        if (isChilled)
        {
            chilledTimer -= Time.deltaTime;
            if (chilledTimer < 0)
            {
                isChilled = false;
                chilledIcon.CloseIcon();
            }
        }
        if (isShocked)
        {
            shockedTimer -= Time.deltaTime;
            if (shockedTimer < 0)
            {
                isShocked = false;
                shockedIcon.CloseIcon();
            }
        }
        if (currentHp <= 0 && (!isDead))
        {
            currentHp = 0;
            Die();
            isDead = true;
        }
    }

    public virtual void IncreaseHp(int amount)
    {
        currentHp += amount;
        currentHp = Mathf.Min(maxHp.GetValue(), currentHp);
    }
    public virtual void IncreaseStatBy(int modifier, float duration, Stat statToModify)
    {
        StartCoroutine(StatModCoroutine(modifier, duration, statToModify));
    }
    private IEnumerator StatModCoroutine(int modifier, float duration, Stat statToModify)
    {
        statToModify.addModifies(modifier);
        Inventory.Instance.UpdateSlotUI();

        yield return new WaitForSeconds(duration);

        statToModify.removeModifies(modifier);
        Inventory.Instance.UpdateSlotUI();
    }

    #region Magical Damage
    public void SetIgnitedDamage(int _IgnitedDamage) => ignitedDamage = _IgnitedDamage;
    public void SetThunderDamage(int _ThunderDamage) => shockedDamage = _ThunderDamage;
    public virtual void DoMagicalDamage(CharacterStats targetStat, int AttackDir = -250)
    {
        if (AttackDir == -250)
        {
            AttackDir = facingDir;
        }
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        int totDamage = _fireDamage + _iceDamage + _lightningDamage;
        totDamage = checkMagicalResistance(targetStat, totDamage);
        targetStat.TakeDamage(totDamage, AttackDir);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) == 0)
        {
            return;
        }

        bool canApplyIngited = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChilled = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShocked = _lightningDamage > _fireDamage && _lightningDamage > _fireDamage;

        while (!canApplyIngited && !canApplyChilled && !canApplyShocked)
        {
            int rand = Random.Range(0, 100);
            if (rand < 33 && _fireDamage > 0)
            {
                canApplyIngited = true;
            }
            else if (rand < 67 && _iceDamage > 0)
            {
                canApplyChilled = true;
            }
            else if (_lightningDamage > 0)
            {
                canApplyShocked = true;
            }
        }

        if (canApplyShocked)
        {
            targetStat.SetThunderDamage(Mathf.RoundToInt(lightningDamage.GetValue() * 0.2f));
        }
        if (canApplyIngited)
        {
            targetStat.SetIgnitedDamage(Mathf.RoundToInt(fireDamage.GetValue() * 0.2f));
        }
        targetStat.ApplyAilment(canApplyIngited, canApplyChilled, canApplyShocked);
    }
    private static int checkMagicalResistance(CharacterStats targetStat, int totDamage)
    {
        totDamage -= targetStat.magicResistance.GetValue() + targetStat.intelligence.GetValue() * 3;
        totDamage = Mathf.Clamp(totDamage, 0, int.MaxValue);
        return totDamage;
    }
    public void ApplyAilment(bool isIgnited, bool isChilled, bool isShocked)
    {


        if (this.isIgnited || this.isChilled || (this.isShocked && !isShocked))
        {
            return;
        }

        if (isIgnited)
        {
            this.isIgnited = isIgnited;
            ignitedTimer = ailmentsDuration;
            ignitedDamgeTimer = 0;
            fx.IgniteFxFor(ailmentsDuration);
            ignitedIcon.OpenIcon();
        }
        if (isChilled)
        {
            this.isChilled = isChilled;
            chilledTimer = ailmentsDuration;
            fx.ChillFxFor(ailmentsDuration);
            float slowPertage = 0.2f;
            GetComponent<Entity>().slowEntityBy(slowPertage, ailmentsDuration);
            chilledIcon.OpenIcon();
        }
        if (isShocked)
        {
            
            if (!this.isShocked)
            {
                shockedIcon.OpenIcon();
                this.isShocked = isShocked;
                shockedTimer = ailmentsDuration;
                fx.ShockFxFor(ailmentsDuration);
            }
            else
            {
                GameObject newshocked = Instantiate(shockedPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity);
                ShockedStrike_Controller shockedScript = newshocked.GetComponent<ShockedStrike_Controller>();
                shockedScript.SetUp(shockedDamage, GetComponent<CharacterStats>());
            }
        }
    }
    #endregion

    #region Physical Damage
    public virtual void DoPhysicalDamage(CharacterStats targetStat, int AttackDir = -250)
    {
        if (AttackDir == -250)
        {
            AttackDir = facingDir;
        }
        if (targetCanAvoideAttack(targetStat))
        {

            return;
        }
        int totDamage = damage.GetValue() + strength.GetValue();
        if (canCrit())
        {
            totDamage = checkCritPowerDamage(totDamage);
        }
        totDamage = checkTargetArmor(targetStat, totDamage);
        targetStat.TakeDamage(totDamage, AttackDir);
    }
    private int checkCritPowerDamage(int totDamage)
    {
        float totCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = totDamage * totCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    private bool targetCanAvoideAttack(CharacterStats targetState)
    {
        int totEvasion = targetState.evasion.GetValue() + targetState.agility.GetValue();
        if (isShocked)
        {
            totEvasion += 20;
        }

        if (Random.Range(0, 100) <= totEvasion)
        {
            return true;
        }
        return false;
    }
    private int checkTargetArmor(CharacterStats targetState, int totDamage)
    {
        int totArmor = targetState.armor.GetValue();
        if (isChilled)
        {
            totDamage -= Mathf.RoundToInt(totArmor * 0.8f);
        }
        return Mathf.Clamp(totDamage, 0, int.MaxValue);
    }
    private bool canCrit()
    {
        int totCritChance = cirtChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) < totCritChance)
        {
            return true;
        }
        return false;
    }
    #endregion

    public virtual void TakeDamage(int _damage, int damageDir)
    {
        DecreaseHealthBy(_damage);
        currentHp -= _damage;
        if (currentHp <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }
    protected virtual void DecreaseHealthBy(int damage)
    {
        currentHp -= damage;
        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }
    protected virtual void Die()
    {
    }
    public int GetMaxHp() => maxHp.GetValue() + vitality.GetValue() * 5;
    public Stat GetStat(StatType statType)
    {
        if (statType == StatType.evasion)
        {
            return evasion;
        }
        else if (statType == StatType.agility)
        {
            return agility;
        }
        else if (statType == StatType.intelligence)
        {
            return intelligence;
        }
        else if (statType == StatType.strength)
        {
            return strength;
        }
        else if (statType == StatType.maxHp)
        {
            return maxHp;
        }
        else if (statType == StatType.magicResistance)
        {
            return magicResistance;
        }
        else if (statType == StatType.armor)
        {
            return armor;
        }
        else if (statType == StatType.cirtChance)
        {
            return cirtChance;
        }
        else if (statType == StatType.critPower)
        {
            return critPower;
        }
        else if (statType == StatType.damage)
        {
            return damage;
        }
        else if (statType == StatType.fireDamage)
        {
            return fireDamage;
        }
        else if (statType == StatType.iceDamage)
        {
            return iceDamage;
        }
        else if (statType == StatType.lightningDamage)
        {
            return lightningDamage;
        }else if (statType == StatType.vitality)
        {
            return vitality;
        }
        return null;
    }
}
