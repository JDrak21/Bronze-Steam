using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if(collision.gameObject.CompareTag("Enemy"))
      {
        collision.gameObject.GetComponent<Enemy>().TakeDamage(2);
      }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
