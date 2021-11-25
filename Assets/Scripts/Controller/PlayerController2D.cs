using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : Controller2D
{
    private const float GROUND_SENSITIVITY = 0.01f;

    // Components
    [SerializeField]
    private InputDataObj inputDataObj = null;
    [SerializeField]
    private BoxCollider2D boxCollider2D = null;
    [SerializeField]
    private Player controlledPlayer = null;

    // Attributes
    [SerializeField]
    private LayerMask groundLayer = 0;
    [SerializeField]
    private float walkSpeed = 4f;
    [SerializeField]
    private float runSpeed = 8f;
    [SerializeField]
    private float accelerate = 16f;
    [SerializeField]
    private float dashForce = 20f;
    [SerializeField]
    private float jumpForce = 12f;

    // Temporary attributes
    private bool isGrounded = false;

    #region Properties
    public bool RunningMode => inputDataObj.isRunningMode;
    public bool AttackCalled => inputDataObj.isAttackStart;
    public bool DashCalled => inputDataObj.isDashStart;
    public bool IsGrounded => isGrounded;

    /// <summary>
    /// Information which the controller control itself
    /// </summary>
    public bool IsSelfMoving => Mathf.Abs(inputDataObj.moveX) > 0f;
    #endregion

    protected override void HandleMovement()
    {
        // Get velocity info
        Vector2 currentVel = controllerRigid.velocity;

        // Movement can oinly be executed when player is on the ground
        DetectGround();
        if (isGrounded && !controlledPlayer.IsDashing)
        {
            // Handle horizontal movement
            if (Mathf.Abs(inputDataObj.moveX) > 0f)
            {
                float moveXDir = inputDataObj.moveX * accelerate * Time.deltaTime;
                currentVel.x += moveXDir;
                
                // Speed cap
                if (RunningMode)
                {
                    if ((currentVel.x > runSpeed && moveXDir > 0f) || (currentVel.x < -runSpeed && moveXDir < 0f))
                        currentVel.x = inputDataObj.moveX * runSpeed;
                }
                else
                {
                    if ((currentVel.x > walkSpeed && moveXDir > 0f) || (currentVel.x < -walkSpeed && moveXDir < 0f))
                        currentVel.x = inputDataObj.moveX * walkSpeed;
                }
            }
            else
            {
                float afterAccelerate;
                if (currentVel.x > 0f) afterAccelerate = currentVel.x - accelerate * Time.deltaTime;
                else if (currentVel.x < 0f) afterAccelerate = currentVel.x + accelerate * Time.deltaTime;
                else afterAccelerate = 0f;

                // Speed cap
                if ((afterAccelerate < 0f && currentVel.x > 0) || (afterAccelerate > 0f && currentVel.x < 0))
                    afterAccelerate = 0f;
                currentVel.x = afterAccelerate;
            }

            // Handle vertical movement
            if (inputDataObj.isJumpStart) currentVel.y = jumpForce;
        }

        // Overwrite velocity info
        controllerRigid.velocity = currentVel;
    }

    private void Dash()
    {
        Vector2 dashDirection;
        if (controlledPlayer.IsFacingRight) dashDirection = Vector2.right;
        else dashDirection = Vector2.left;

        // Assign dash
        dashDirection.x *= dashForce;
        dashDirection.y = controllerRigid.velocity.y;
        controllerRigid.velocity = dashDirection;
    }

    private void DetectGround()
    {
        Vector2 boxSize = boxCollider2D.bounds.size;
        Vector2 origin = boxCollider2D.bounds.center.ToV2() + new Vector2(0f, -boxSize.y / 2f - GROUND_SENSITIVITY);
        RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(boxSize.x, GROUND_SENSITIVITY), 0f, 
            Vector2.down, GROUND_SENSITIVITY, groundLayer);
        isGrounded = hit.collider != null;

        #if UNITY_EDITOR
        if (debugMode)
        {
            Vector2 topLeftPoint = origin + new Vector2(-boxSize.x /2f, -GROUND_SENSITIVITY / 2f);
            Color detectColor = isGrounded ? Color.green : Color.red;
            Debug.DrawRay(topLeftPoint, Vector3.down * GROUND_SENSITIVITY, detectColor); // Left side
            Debug.DrawRay(topLeftPoint, Vector3.right * boxSize.x, detectColor); // Upper side
            Debug.DrawRay(topLeftPoint + Vector2.down * GROUND_SENSITIVITY, Vector3.right * boxSize.x, detectColor); // Lower side
            Debug.DrawRay(topLeftPoint + Vector2.right * boxSize.x, Vector3.down * GROUND_SENSITIVITY, detectColor); // Right side
        }
        #endif
    }
}
