using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "NightShot/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public string weaponName;

    public enum weaponType
    {
        hitscanRay,
        hitscanZone,
        projectile,
    }

    [Header("Weapon Type")]
    public weaponType type;
    
    [Header("Weapon data")]
    public int attackDamage = 1;
    public int burstCount = 1;
    public int maxAmmo = 5;
    [HideInInspector]
    public int ammo;
    public int numberOfUsageMax = 3;
    public int numberOfUsage = 3;
    public float fireRate = 0.5f;
    public float burstSpacing = 0.05f;
    public float dispertion = 0.3f;
    public float bulletTravelTime = 0.8f;
    public float angle = 20f;
    public float weaponRange = 20f;
    public float projectileSpeed = 1f;
    public bool constantSpeed = true;

    [Header("Weapon visual")]
    public Object weaponMesh;
    public Vector3 firePointOffset = new Vector3 (0f,0.15f,0.75f);
    public Object weaponMaterial;
    public Object projectilePrefab;
    public Object muzzleFlash;
    public Object impactEffect;
    public Object bulletTrail;

    public bool canFire = true;

    public void ReduceUsage()
    {
        numberOfUsage--;
    }

    public void Reload()
    {
        ammo = maxAmmo;
    }

    public IEnumerator Fire(Transform origin)
    {
        canFire = false;
        
        for (int i = burstCount; i>0; i--)
        {
            GameObject muzzleFlashObject = (GameObject)Instantiate(muzzleFlash, origin);
            Destroy(muzzleFlashObject, muzzleFlashObject.GetComponent<ParticleSystem>().main.duration);

            switch (type)
            {
                case weaponType.hitscanRay:
                    ammo--;

                    GameObject trailObject = (GameObject)Instantiate(bulletTrail);
                    trailObject.transform.position = origin.position;

                    Vector3 directionRay = origin.transform.forward;
                    directionRay.x += Random.Range(-dispertion, dispertion);
                    directionRay.y += Random.Range(-dispertion, dispertion);
                    directionRay.z += Random.Range(-dispertion, dispertion);
                    Physics.Raycast(origin.position, directionRay, out RaycastHit hit);

                    if (i > 0)
                        yield return new WaitForSeconds(bulletTravelTime);


                    if (hit.collider != null)
                    {
                        Debug.Log(hit.collider.gameObject.name);

                        hit.collider.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController);
                        if (playerController != null)
                        {
                            Debug.Log(playerController.gameObject.name + " hit");
                            playerController.TakeDamage(attackDamage);
                        }

                            GameObject impactEffectObject = (GameObject)Instantiate(impactEffect);
                        Destroy(impactEffectObject, impactEffectObject.GetComponent<ParticleSystem>().main.duration);
                        
                        trailObject.transform.position = hit.point;
                        impactEffectObject.transform.position = hit.point;
                    }

                    else
                    {
                        trailObject.transform.position = origin.position + origin.forward*weaponRange;
                    }

                    break;


                case weaponType.projectile:
                    ammo--;
                    GameObject projectileObject = (GameObject)Instantiate(projectilePrefab);
                    projectileObject.GetComponent<Projectile>().weapon = this;
                    projectileObject.transform.position = origin.position;
                    projectileObject.transform.rotation = origin.rotation;
                    break;

            }

            if (i > 0)
                yield return new WaitForSeconds(burstSpacing);
        }

        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}
