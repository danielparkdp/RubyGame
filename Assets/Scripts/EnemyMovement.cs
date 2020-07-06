using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

      public float speed;
      public bool vertical;
      public float changeTime = 2.0f;
      bool broken = true;
      public ParticleSystem smokeEffect;
      AudioSource audioSource;
      //public AudioClip brokenClip;
      public AudioClip fixedClip;

      Rigidbody2D rigidbody;
      float timer;
      int direction = 1;
      Animator animator;

      // Start is called before the first frame update
      void Start()
      {
          rigidbody = GetComponent<Rigidbody2D>();
          animator = GetComponent<Animator>();
          audioSource = GetComponent<AudioSource>();
          timer = changeTime;
      }


      void Update()
      {
          if (!broken){
            return;
          }

          timer -= Time.deltaTime;

          if (timer < 0)
          {
              direction = -direction;
              timer = changeTime;
          }
      }

      void FixedUpdate()
      {
          if (!broken){
            return;
          }

          Vector2 position = rigidbody.position;

          if (vertical)
          {
              position.y = position.y + Time.deltaTime * speed * direction;
              animator.SetFloat("Move X", 0);
              animator.SetFloat("Move Y", direction);
          }
          else
          {
              position.x = position.x + Time.deltaTime * speed * direction;
              animator.SetFloat("Move X", direction);
              animator.SetFloat("Move Y", 0);
          }

          rigidbody.MovePosition(position);
      }

      void OnCollisionEnter2D(Collision2D other)
      {
          RubyController player = other.gameObject.GetComponent<RubyController >();

          if (player != null)
          {
              player.ChangeHealth(-1);
          }
      }

      public void Fix(){
        broken = false;
        rigidbody.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        audioSource.clip = fixedClip;
        audioSource.Play();
      }
}
