using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
    public Weapon thrownWeapon;
    [SerializeField] GameObject mesh;
    [SerializeField] float throwForce = 100f;
    bool isPickable = true;
    Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * throwForce;
        mesh.GetComponent<MeshFilter>().mesh = thrownWeapon.weaponMesh as Mesh;
        mesh.GetComponent<MeshRenderer>().material = thrownWeapon.weaponMaterial as Material;
    }

    private void OnCollisionEnter (Collision collision)
    {
        collision.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController);
        if (playerController != null)
        {
            if(isPickable)
            {
                if (thrownWeapon.numberOfUsage > 1)
                {
                    playerController.ChangeWeapon(thrownWeapon);
                    Destroy(gameObject);
                }

                else
                {
                    Destroy(thrownWeapon);
                    Destroy(gameObject);
                }
            }

            else
            {
                if (thrownWeapon.numberOfUsage > 1)
                    Debug.Log("J'ai été touché par l'arme " + thrownWeapon.ToString());

                else
                    Destroy(gameObject);
            }
        }

    }
}
