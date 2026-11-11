
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] WeaponData weapon;
    [SerializeField] AudioClip PickUpSound;
    [SerializeField] AudioSource audioSource;

    void OnTriggerEnter(Collider other)
    {
        Shoot controller =
            other.GetComponentInParent<Shoot>();

        if (controller != null)
        {
            controller.SetWeapon(weapon);

            gameObject.SetActive(false);
        }
        if (audioSource != null && PickUpSound != null)
        {
            audioSource.PlayOneShot(PickUpSound);
        }
    }
}
