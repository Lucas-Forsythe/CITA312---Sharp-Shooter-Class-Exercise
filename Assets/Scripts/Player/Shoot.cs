using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float range = 100f;
    void Shooting()
    {
        RaycastHit hit;
         if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit,
         range))
         {
         Debug.Log("Hit: " + hit.transform.name);
         }
        }
}
