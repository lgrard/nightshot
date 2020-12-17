using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField] Weapon weaponToPick;
    private Weapon weaponToPickInstance;
    [SerializeField] GameObject mesh;

    private void Start()
    {
        weaponToPickInstance = Object.Instantiate(weaponToPick);

        mesh.GetComponent<MeshFilter>().mesh = weaponToPickInstance.weaponMesh as Mesh;
        mesh.GetComponent<MeshRenderer>().material = weaponToPickInstance.weaponMaterial as Material;

        weaponToPick.numberOfUsage = weaponToPickInstance.numberOfUsageMax;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController);
        if (playerController != null)
        {
            playerController.ChangeWeapon(weaponToPickInstance);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(gameObject.transform.position,0.25f);
    }
}
