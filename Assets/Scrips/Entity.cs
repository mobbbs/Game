using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockBackDirction;
    [SerializeField]protected float KnockbackDuration;
    protected bool isKnocked;

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDis;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDis;

    #region Component
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public EntityFx fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion
    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;

    public System.Action onFlipped;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFx>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {

    }
    
    public virtual void slowEntityBy(float slowPertage, float slowDuration)
    {

    }
    protected virtual void ReturnDefautSpeed()
    {
        anim.speed = 1;
    }
    public virtual void Damage(int HitKnockDir = 0)
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine(HitKnockback(HitKnockDir));
    }
    protected virtual IEnumerator HitKnockback(int HitKnockDir)
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockBackDirction.x * HitKnockDir, knockBackDirction.y);

        yield return new WaitForSeconds(KnockbackDuration);
        isKnocked = false;
    }
    #region Velocity
    public void SetZeroVelocity()
    {   
        if (isKnocked)
        {
            return;
        }
        rb.velocity = new Vector2(0, 0);
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDis, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDis, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDis));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDis * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
        {
            onFlipped();
        }
    }
    public virtual void FlipController(float x)
    {
        if (x > 0 && !facingRight)
        {
            Flip();
        }
        else if (x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    public virtual void Die()
    {

    }
}
