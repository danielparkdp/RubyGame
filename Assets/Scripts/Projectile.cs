using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force){
        rigidbody.AddForce(direction * force);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1000.0f){
          Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        EnemyMovement e = other.collider.GetComponent<EnemyMovement>();
        if (e != null){
            e.Fix();
        }


        Destroy(gameObject);
    }
}
