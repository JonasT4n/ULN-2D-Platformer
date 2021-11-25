using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private InputDataObj inputDataObj = null;

    #region Unity BuiltIn Methods
    private void Update()
    {
        inputDataObj.moveX = Input.GetAxis("Horizontal"); // A & S or Joystick X Axis to Move Left Right
        inputDataObj.isJumpStart = Input.GetKeyDown(KeyCode.Space); // Space to Jump
        inputDataObj.isDashStart = Input.GetKeyDown(KeyCode.Mouse1); // Right Click Mouse to Dash
        inputDataObj.isRunningMode = Input.GetKey(KeyCode.LeftShift); // Left Shift to activate running mode
        inputDataObj.isAttackStart = Input.GetKeyDown(KeyCode.Mouse0); // Left Click Mouse to Attack
    }
    #endregion
}
