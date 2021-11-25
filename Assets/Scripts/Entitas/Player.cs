using UnityEngine;

[RequireComponent(typeof(PlayerController2D))]
public class Player : MakhlukHidup
{
    // Components
    [SerializeField]
    private PlayerController2D controller = null;
    [SerializeField]
    private BoxCollider2D hitArea = null;
    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    // Attributes
    [SerializeField]
    private LayerMask collectableMask = 0;
    [SerializeField]
    private LayerMask targetHitMask = 0;

    // Temporary variables
    private bool isDashing = false;
    private bool isAttacking = false;

    #region Properties
    public bool IsFacingRight => !spriteRenderer.flipX;
    public bool IsDashing => isDashing;
    public bool IsAttacking => isAttacking;
    #endregion

    #region Unity BuiltIn Methods
    private void Update()
    {
        // Update animation
        animator.SetFloat("MoveXDirection", controller.MoveVelocity.x);
        animator.SetBool("IsRunning", controller.RunningMode);
        animator.SetBool("IsGrounded", controller.IsGrounded);
        animator.SetBool("IsAttacking", controller.AttackCalled);
        animator.SetBool("IsSelfMoving", controller.IsSelfMoving);
        animator.SetBool("IsDashing", controller.DashCalled);
    }

    private void OnTriggerEnter2D(Collider2D triggerHit)
    {
        if (collectableMask == (collectableMask | (1 << triggerHit.gameObject.layer)))
        {
            MedKit medKit = triggerHit.GetComponent<MedKit>();
            if (medKit != null)
            {
                medKit.Collect();
                health += medKit.Heal;
            }
        }
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
