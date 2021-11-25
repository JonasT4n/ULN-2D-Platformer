using UnityEngine;

[CreateAssetMenu(fileName = "Input Data", menuName = "Technical/Input Data")]
public class InputDataObj : ScriptableObject
{
    public float moveX = 0f;
    public bool isJumpStart = false;
    public bool isRunningMode = false;
    public bool isDashStart = false;
    public bool isAttackStart = false;
    
    #region Unity BuiltIn Methods
    public void Reset()
    {
        moveX = 0f;
        isJumpStart = false;
        isRunningMode = false;
        isDashStart = false;
        isAttackStart = false;
    }
    #endregion
}
