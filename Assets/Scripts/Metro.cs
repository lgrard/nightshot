using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metro : MonoBehaviour
{
    [SerializeField, Range(0f, 50f)] private float speed = 20f;
    [SerializeField, Range(0f, 50f)] private float lifeTime = 10f;
    [SerializeField] int damage = 10;

    private float       timer = 0.0f;
    private Rigidbody   rb;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        

        rb.velocity = transform.right * speed;
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }        
    }

    private void OnTriggerEnter(Collider collision)
    {

        collision.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController);

        if(playerController != null)
        {
            playerController.TakeDamage(damage);
        }
    }
}
