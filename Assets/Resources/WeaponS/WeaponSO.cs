using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string Type;
    public float Power;
    public float Speed;
    public float Range;
    public bool flippable;
    public GameObject[] LightAttacks;
    public GameObject[] HeavyAttacks;

}
