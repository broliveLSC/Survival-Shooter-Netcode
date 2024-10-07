using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

public class EnemyMovement : NetworkBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;
    float targetTimer = 1;

    void Awake ()
    {
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    }


    void Update ()
    {
        if (!IsServer)
            return;

        targetTimer -= Time.deltaTime;
        if(targetTimer <= 0)
        {
            targetTimer = 1;

            if (!TargetClosestPlayer())
                nav.enabled = false;
        }

    }

    public bool TargetClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDist = 9999f;
        PlayerHealth[] players = FindObjectsOfType<PlayerHealth>();
        foreach (PlayerHealth p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < closestDist)
            {
                closestPlayer = p.transform;
                closestDist = dist;
            }
        }

        if (closestPlayer != null)
        {
            //nav.enabled = true;
            nav.SetDestination(closestPlayer.position);
            return true;
        }

        return false;
    }
}
