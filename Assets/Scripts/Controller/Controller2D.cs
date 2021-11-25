using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Controller2D : MonoBehaviour
{
    // Components
    [SerializeField]
    protected Rigidbody2D controllerRigid = null;

    #if UNITY_EDITOR
    [SerializeField]
    protected bool debugMode = false;
    #endif

    #region Properties
    public Vector2 MoveVelocity => controllerRigid.velocity;
    #endregion

    #region Unity BuiltIn Methods
    protected virtual void Update() => HandleMovement();
    #endregion

    protected abstract void HandleMovement();
}
