using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AIController2D : Controller2D
{
    // Components
    [SerializeField]
    private Enemy controlledEnemy = null;
    [SerializeField]
    private Transform eyes = null;

    // Attributes
    [SerializeField]
    private Vector2[] patrolPoints = null;
    [SerializeField]
    private LayerMask targetLayer = 0;
    [SerializeField]
    private float lostTargetDistance = 5f;
    [SerializeField]
    private float chaseSpeed = 0f;
    [SerializeField]
    private float attackInterval = 2f;
    [SerializeField]
    private float farSight = 5f;
    [SerializeField]
    private float stopDistance = 0f;
    [SerializeField]
    [Tooltip("AI will target to next index of patrol point, and keep the loop")]
    private float moveToNextPointInterval = 5f;

    // Temporary attributes
    private IEnumerator currentAttackRoutine = null;
    private Transform targetLocked = null;
    private Vector2 targetPosition = Vector2.zero;
    private int patrolPointIndex = -1;
    private float tempAttackSecond = 0f;
    private float tempPatrolSecond = 0f;
    private bool isAttackCalled = false;

    #region Properties
    public bool AttackCalled => isAttackCalled;
    public bool IsTargetLocked => targetLocked != null;
    #endregion

    #region Unity BuiltIn Methods
    protected override void Update()
    {
        base.Update();
        HandleFindTarget();
    }
    #endregion

    protected override void HandleMovement()
    {
        // No target to chase, then patrol
        float stopDistanceOverride;
        if (controlledEnemy.IsInDanger)
        {
            // Flee from target
            HandleFlee();
            stopDistanceOverride = 0f;
        }
        else if (targetLocked == null)
        {
            // No target, then patrol
            HandlePatrolMovement();
            stopDistanceOverride = 0f;
        }
        else
        {
            // Has target, then chase
            targetPosition = targetLocked.transform.position;
            stopDistanceOverride = stopDistance;
        }

        // Check lost target
        float distanceBetween = Vector2.Distance(targetPosition, transform.position);
        if (distanceBetween > lostTargetDistance && targetLocked != null) targetLocked = null;
        else if (distanceBetween <= stopDistanceOverride && targetLocked != null) HandleAutoAttack();

        // Handle movement, only X Axis will affect
        float moveXDir = targetPosition.x - transform.position.x < 0f ? -1f : 1f;
        distanceBetween = Mathf.Abs(targetPosition.x - transform.position.x) - stopDistanceOverride;
        if (distanceBetween > 0)
        {
            float currentXVel = moveXDir * chaseSpeed;
            float afterXMove = transform.position.x + currentXVel * Time.fixedDeltaTime;
            float distanceAfterMove = Mathf.Abs(targetPosition.x - afterXMove) - stopDistanceOverride;
            if (distanceAfterMove > distanceBetween) currentXVel = targetPosition.x - transform.position.x;
            controllerRigid.velocity = new Vector2(currentXVel, controllerRigid.velocity.y);
        }
    }

    private void HandleFindTarget()
    {
        // Define look direction
        Vector2 lookDirection;
        if (controlledEnemy.IsFacingRight) lookDirection = Vector2.right;
        else lookDirection = Vector2.left;

        // Looking for target
        RaycastHit2D hit = Physics2D.Raycast(eyes.position, lookDirection, farSight, targetLayer);
        targetLocked = hit.transform;
        if (targetLocked != null) targetPosition = targetLocked.transform.position;

        #if UNITY_EDITOR
        if (debugMode)
        {
            Color targetFoundColor = targetLocked != null ? Color.green : Color.red;
            Debug.DrawRay(eyes.position, lookDirection * farSight, targetFoundColor);
        }
        #endif
    }

    private void HandlePatrolMovement()
    {
        // Move to next point after amount of time
        tempPatrolSecond -= Time.deltaTime;
        if (tempPatrolSecond <= 0f)
        {
            tempPatrolSecond = moveToNextPointInterval;
            if (patrolPoints.Length > 0)
            {
                patrolPointIndex = patrolPointIndex + 1 >= patrolPoints.Length ? 0 : patrolPointIndex + 1;
                targetPosition = patrolPoints[patrolPointIndex];
            }
        }
    }

    private void HandleAutoAttack()
    {
        tempAttackSecond -= Time.deltaTime;
        if (tempAttackSecond <= 0f && targetLocked != null)
        {
            // Reset attack
            tempAttackSecond = attackInterval;

            // Run attack routine
            if (currentAttackRoutine != null) StopCoroutine(currentAttackRoutine);
            currentAttackRoutine = HandleAttackFrame();
            StartCoroutine(currentAttackRoutine);
        }
    }

    private void HandleFlee()
    {
        // Flee from target
        if (targetLocked != null)
        {
            Vector2 fleeFromPos = targetLocked.position;
            if (fleeFromPos.x > transform.position.x) targetPosition.x = float.MaxValue;
            else targetPosition.x = float.MinValue;
        }
    }

    private IEnumerator HandleAttackFrame()
    {
        isAttackCalled = true;
        yield return null;
        isAttackCalled = false;
        currentAttackRoutine = null;
    }
}
