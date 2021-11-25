using UnityEngine;

public class MedKit : MonoBehaviour, ICollectableObject
{
    [SerializeField]
    private float healPoint = 25f;

    #region Properties
    public float Heal => healPoint;
    #endregion

    public void Collect() => Destroy(this.gameObject);
}
