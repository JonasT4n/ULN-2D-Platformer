using UnityEngine;

[RequireComponent(typeof(AIController2D))]
public class Enemy : MakhlukHidup
{
    [SerializeField]
    private AIController2D controller = null;
    [SerializeField]
    private BoxCollider2D hitArea = null;
    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    // Attributes
    [SerializeField]
    private LayerMask targetHitMask = 0;
    [SerializeField]
    [Tooltip("If enemy has HP less or equal than this, then the enemy will flee")]
    private float fleeAtHP = 20f;

    // Temporary variables
    private bool isDashing = false;
    private bool isAttacking = false;

    #region Properties
    public bool IsFacingRight => !spriteRenderer.flipX;
    public bool IsInDanger => health <= fleeAtHP;
    #endregion

    #region Unity BuiltIn Methods
    private void Start()
    {
        // AI always running and self moving to chase target
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsSelfMoving", true);
    }

    private void Update()
    {
        animator.SetFloat("MoveXDirection", controller.MoveVelocity.x);
        animator.SetBool("IsAttacking", controller.AttackCalled);
    }
    #endregion

    public override void Die()
    {
        animator.SetBool("IsDead", true);
        controller.enabled = false;
    }

    protected override void MeleeAttack()
    {
        // Detect hit
        RaycastHit2D[] hits = Physics2D.BoxCastAll(hitArea.bounds.center, hitArea.bounds.size, 0f, Vector2.zero, targetHitMask);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.collider != null)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null) enemy.GetDamage(damage);
                }
            }
        }
    }

    private void SetIsDashingState(int isDashing) => this.isDashing = isDashing > 0;
    private void SetIsAttackingState(int isAttacking) => this.isAttacking = isAttacking > 0;
    
}
