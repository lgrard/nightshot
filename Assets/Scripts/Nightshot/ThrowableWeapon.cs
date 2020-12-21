﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThrowableWeapon : MonoBehaviour
{
    [HideInInspector]
    public Weapon thrownWeapon;

    [Header("Visual")]
    [SerializeField] GameObject mesh;
    [SerializeField] GameObject indicatorPrefab;
    GameObject indicator;

    [Header("Datas")]
    [SerializeField] float throwForce = 100f;
    [SerializeField] float minimumHitForce = 80f;
    [SerializeField] float dropRadius;
    [SerializeField] float dropHeight = 3;
    [SerializeField] float dropTime = 2;
    [SerializeField] float meshRotationSpeed = 2;
    public bool isThrown = true;

    [Header("Curves")]
    [SerializeField] AnimationCurve horizontalDropCurve;
    [SerializeField] AnimationCurve verticalDropCurve;

    [Header("Components")]
    bool isPickable = false;
    Rigidbody rb;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        mesh.GetComponent<MeshFilter>().mesh = thrownWeapon.weaponMesh as Mesh;
        mesh.GetComponent<MeshRenderer>().material = thrownWeapon.weaponMaterial as Material;

        mesh.transform.Rotate(new Vector3(Random.Range(0,100), Random.Range(0, 100), Random.Range(0, 100)));

        if(isThrown)
            rb.velocity = transform.forward * throwForce;

        else
            StartCoroutine(Bounce());
    }

    private void OnTriggerEnter(Collider collision)
    {
        collision.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController);

        if (thrownWeapon.numberOfUsage >= 1)
        {
            if (playerController != null)
            {
                if (isPickable)
                {
                    playerController.ChangeWeapon(thrownWeapon);
                    Despawn(false);
                }

                else if (rb.velocity.magnitude >= minimumHitForce)
                    StartCoroutine(Bounce());
            }

            else
            {
                Replace(transform.position);
                isPickable = true;
            }
        }

        else
            Despawn(true);
    }


    void Despawn(bool destroyWeapon)
    {
        Destroy(gameObject);

        if (destroyWeapon)
        {
            Debug.Log("Weapon destroyed");
            Destroy(thrownWeapon);        
        }
        
        if(indicator!= null)
            Destroy(indicator);
    }

    void Replace(Vector3 origin)
    {
        rb.isKinematic = true;

        NavMeshHit hit;
        NavMesh.SamplePosition(origin, out hit, dropRadius, 1);
        transform.position = hit.position + new Vector3(0, 0.4f, 0);
    }

    IEnumerator Bounce()
    {
        rb.isKinematic = true;

        Vector3 lastPosition = transform.position;
        Vector3 nextPosition = (Random.insideUnitCircle.normalized* dropRadius);
        nextPosition += lastPosition;
        NavMeshHit hit;

        NavMesh.SamplePosition(nextPosition, out hit, dropRadius*2, 1);
        nextPosition = hit.position + new Vector3(0,0.4f,0);

        indicator = GameObject.Instantiate(indicatorPrefab);
        indicator.transform.position = hit.position;
        Material indicatorMaterial = indicator.GetComponentInChildren<MeshRenderer>().material;

        float timeSinceImpact = dropTime;
        while(timeSinceImpact > 0)
        {
            Vector3 currentPosition;
            float progressionValue = (dropTime - timeSinceImpact) / dropTime;

            currentPosition.x = Mathf.Lerp(lastPosition.x,nextPosition.x, horizontalDropCurve.Evaluate(progressionValue));
            currentPosition.z = Mathf.Lerp(lastPosition.z, nextPosition.z, horizontalDropCurve.Evaluate(progressionValue));
            currentPosition.y = Mathf.Lerp(nextPosition.y + dropHeight, nextPosition.y, verticalDropCurve.Evaluate(progressionValue));

            indicatorMaterial.SetFloat("progress", progressionValue);
            transform.position = currentPosition;
            timeSinceImpact -= Time.deltaTime;

            if(progressionValue >= 0.5)
                isPickable = true;

            mesh.transform.Rotate(Vector3.one* meshRotationSpeed);
            indicator.transform.position = nextPosition;

            yield return new WaitForFixedUpdate();
        }

        Destroy (indicator);
    }
}
