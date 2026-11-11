
using UnityEngine;
using UnityEngine.Audio;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int amount = 10;
    [SerializeField] AudioClip PickUpSound;
    [SerializeField] AudioSource audioSource;

    void OnTriggerEnter(Collider other)
    {
        Shoot controller =
            other.GetComponentInParent<Shoot>();

        if (controller != null)
        {
            controller.AddAmmo(amount);

            gameObject.SetActive(false);

            Debug.Log("Ammo pickup collected!");

            if (audioSource != null && PickUpSound != null)
            {
                audioSource.PlayOneShot(PickUpSound);
            }
        }
    }
}
