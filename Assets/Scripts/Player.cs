using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 4.0f;
    Rigidbody2D rb;

    private float health = 100;

    public bool turnedLeft = false;
    public Image healthFill;
    private float healthWidth;

    public Text mainText;

    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        healthWidth = healthFill.sprite.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal=Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        rb.velocity=new Vector2(horizontal * speed, vertical * speed);
        turnedLeft = false;

        if(horizontal > 0)
        {
          GetComponent<Animator>().Play("Walk");
        } else if (horizontal < 0)
        {
          GetComponent<Animator>().Play("Walk Back");
          turnedLeft = true;
        } else if (vertical > 0)
        {
          GetComponent<Animator>().Play("Walk");
        } else if (vertical < 0)
        {
          GetComponent<Animator>().Play("Walk");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.CompareTag("Enemy")||collision.gameObject.CompareTag("SharkFace")|| collision.gameObject.CompareTag("EnforcerRagned"))
      {
        transform.GetChild(0).gameObject.SetActive(true);
        health -= collision.gameObject.GetComponent<Enemy>().GetHitStrength();
        if(health < 1)
        {
          healthFill.enabled = false;
          mainText.gameObject.SetActive(true);
          mainText.text = "Game Over";
        }
        Vector2 temp = new Vector2(healthWidth * (health/100), healthFill.sprite.rect.height);
        healthFill.rectTransform.sizeDelta = temp;
        Invoke("HidePlayerBlood", 0.25f);
      }
    }

    void HidePlayerBlood()
    {
      transform.GetChild(0).gameObject.SetActive(false);
    }
}
