
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    [Header("Weapon Stats")]
    public int damage = 1;
    public float range = 100f;
    public float cooldown = 0.25f;
    public int magazineSize = 12;
    public bool automatic;
    public float zoomFOV = 30f;

    [Header("Weapon Audio")]
    public AudioClip shootingSound;
}
