using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{

    public int maxHealth = 5;
    public float speed = 3.5f;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    public GameObject damageParticle;
    AudioSource audioSource;
    public AudioClip launchClip;

    public int health { get { return currentHealth; }}
    int currentHealth;
    Rigidbody2D rigidbody;
    float horizontal;
    float vertical;
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal,vertical);
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)){
          lookDirection.Set(move.x, move.y);
          lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible){
          invincibleTimer -= Time.deltaTime;
          if (invincibleTimer < 0){
            isInvincible = false;
          }
        }

        if (Input.GetKeyDown(KeyCode.C)){
          Launch();
        }

        if (Input.GetKeyDown(KeyCode.X)){
          RaycastHit2D hit = Physics2D.Raycast(rigidbody.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
          if (hit.collider != null) {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null) {
              character.DisplayDialog();
            }
          }
        }

    }

    void FixedUpdate()
    {
      Vector2 position = transform.position;
      position.x = position.x + speed * horizontal * Time.deltaTime;
      position.y = position.y + speed * vertical * Time.deltaTime;

      rigidbody.MovePosition(position);
    }

    void Launch(){
      GameObject projectileObject = Instantiate(projectilePrefab, rigidbody.position + Vector2.up * 0.5f, Quaternion.identity);
      Projectile projectile = projectileObject.GetComponent<Projectile>();
      projectile.Launch(lookDirection, 300);

      this.PlaySound(launchClip);

      animator.SetTrigger("Launch");
    }

    public void ChangeHealth(int amount)
    {
      if (amount < 0){
        if (isInvincible){
          return;
        }
        GameObject damageObject = Instantiate(damageParticle, rigidbody.position + Vector2.up * 0.5f, Quaternion.identity);
        isInvincible = true;
        invincibleTimer = timeInvincible;
        animator.SetTrigger("Hit");
      }
      currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
      UIHealthBar.instance.SetValue(currentHealth / (float) maxHealth);
    }

    public void PlaySound(AudioClip clip){
      audioSource.PlayOneShot(clip);
    }
}
