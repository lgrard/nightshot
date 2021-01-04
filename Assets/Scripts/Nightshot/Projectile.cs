using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public Weapon weapon;

    private bool constantSpeed = true;
    private float speed = 1f;
    private int damage;
    private GameObject impactEffect;

    Rigidbody rb;

    private void Start()
    {
        damage = weapon.attackDamage;
        speed = weapon.projectileSpeed;
        impactEffect = weapon.impactEffectObject as GameObject;
        constantSpeed = weapon.constantSpeed;

        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        if (constantSpeed)
            rb.velocity = transform.forward * speed;
    }


    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController);
        if (playerController != null)
        {
            playerController.TakeDamage(damage);
        }

        GameObject impactEffectObject = (GameObject)Instantiate(impactEffect);
        Destroy(impactEffectObject, impactEffectObject.GetComponent<ParticleSystem>().main.duration);
        impactEffectObject.transform.position = transform.position;

        Destroy(gameObject);
    }
}
