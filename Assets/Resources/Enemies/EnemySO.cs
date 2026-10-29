using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string Name;
    public float Health;
    public WeaponSO Weapon;
    public float MovementSpeed;
    public bool Follow;
    //[SerializeField] private Transform m_playerTransform;
}
