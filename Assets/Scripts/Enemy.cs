using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float range;
    public Transform target;
    private float minDistance = 5.0f;
    private bool targetCollision = false;
    public float speed = 2.0f;
    private float thrust = 1.5f;
    public int health = 5;
    private int hitStrength = 25;

    public Sprite deathSprite;
    public Sprite[] sprites;

    private bool isDead = false;

    public Animator enemyanimator;
    public Animator sharkfaceanimator;
    public Animator EnforcerRangedanimator;

    void Start()
    {
      //int rnd = Random.Range(0, sprites.Length);
      //GetComponent<SpriteRenderer>().sprite = sprites[rnd];
        if (this.gameObject.tag == "SharkFace")
        {
            sharkfaceanimator.SetBool("Idle", true);
        }
        else if (this.gameObject.tag == "EnforcerRanged")
        {
            EnforcerRangedanimator.SetBool("Idle", true);
        }
        else if (this.gameObject.tag == "Enemy")
        {
            enemyanimator.SetBool("Idle", true);
            enemyanimator.SetBool("Attack", false);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        range=Vector2.Distance(transform.position, target.position);
        Debug.Log("Distancia enemigo básico" + range);
        if(range > 0.86f || range < 0.92f)
        {
            enemyanimator.SetBool("Idle", false);
            enemyanimator.SetBool("Attack", true);
           
        }
        else
        {
            enemyanimator.SetBool("Idle", true);
            enemyanimator.SetBool("Attack", false);
        }
        
        if(range < minDistance && !isDead)
        {
          if (!targetCollision)
          {
            //Get the position of the player
            transform.LookAt(target.position);
            //Correct Rotation
            transform.Rotate(new Vector3(0,-90,0), Space.Self);
            transform.Translate(new Vector3(speed * Time.deltaTime,0,0));
          }
        }
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.CompareTag("Player") && !targetCollision)
      {
        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = collision.collider.bounds.center;

        targetCollision = true;

        bool right = contactPoint.x > center.x;
        bool left = contactPoint.x < center.x;
        bool top = contactPoint.y > center.y;
        bool bottom = contactPoint.y < center.y;

        if(right) GetComponent<Rigidbody2D>().AddForce(transform.right * thrust, ForceMode2D.Impulse);
        if(left) GetComponent<Rigidbody2D>().AddForce(-transform.right * thrust, ForceMode2D.Impulse);
        if(top) GetComponent<Rigidbody2D>().AddForce(transform.up * thrust, ForceMode2D.Impulse);
        if(bottom) GetComponent<Rigidbody2D>().AddForce(-transform.up * thrust, ForceMode2D.Impulse);
        Invoke("FalseCollision", 0.5f);
      }
    }

    void FalseCollision()
    {
      targetCollision = false;
      GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void TakeDamage(int amount)
    {
      health -= amount;
      if(health < 0)
      {
        isDead = true;
        GetComponent<SpriteRenderer>().sprite = deathSprite;
        GetComponent<Collider2D>().enabled = false;
        Invoke("EnemyDeath", 1.0f);
      }
      transform.GetChild(0).gameObject.SetActive(true);
      Invoke("HideBlood", 0.25f);
    }

    void HideBlood()
    {
      transform.GetChild(0).gameObject.SetActive(false);
    }

    void EnemyDeath()
    {
      Destroy(transform.GetChild(0).gameObject);
      Destroy(gameObject);
    }
    public int GetHitStrength()
    {
      return hitStrength;
    }
}
