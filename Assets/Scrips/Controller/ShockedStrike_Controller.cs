using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockedStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;
    private Animator anim;
    private bool triggered;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    public void SetUp(int damage, CharacterStats targetStats)
    {
        this.damage = damage;
        this.targetStats = targetStats;
    }
    private void Update()
    {
        if (targetStats == null || triggered || targetStats.GetComponent<Enemy>() == null)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;
        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            triggered = true;
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            transform.position += new Vector3(0, 1);
            anim.SetTrigger("Hit");
            Invoke("ApplyShockAndSelfDestroy", 0.65f);
        }
    }

    private void ApplyShockAndSelfDestroy()
    {
        targetStats.TakeDamage(damage, 0);
        Destroy(gameObject, 0.5f);
    }
}
