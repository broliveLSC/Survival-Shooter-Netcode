using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    Animator anim;
    GameObject player;
    EnemyHealth enemyHealth;
    float timer;


    void Awake ()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //playerInRange = true;
            player = other.gameObject;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if (player == null)
            return;

        if(other.gameObject == player)
        {
            //playerInRange = false;
            player = null;
        }
    }


    void Update ()
    {
        /*if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        if (playerHealth == null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            return;
        }*/

        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && player != null && enemyHealth.currentHealth.Value > 0)
        {
            Attack ();
        }

        /*if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }*/
    }


    void Attack ()
    {
        var playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth == null)
            return;

        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }

        if (playerHealth.currentHealth < 0)
            player = null;

    }
}
