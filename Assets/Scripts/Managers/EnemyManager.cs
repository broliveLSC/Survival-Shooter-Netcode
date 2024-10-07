using UnityEngine;
using Unity.Netcode;

public class EnemyManager : NetworkBehaviour
{
    GameManager manager;
    PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;


    void Start ()
    {
        //if(IsHost || IsServer)
            InvokeRepeating ("HandleSpawn", spawnTime, spawnTime);

        manager = FindObjectOfType<GameManager>();
    }

    void HandleSpawn()
    {
        if (IsHost || NetworkManager.Singleton.IsHost)
            Spawn();
    }

    void Spawn ()
    {
        /*if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
            return;
        }

        if(playerHealth.currentHealth <= 0f)
        {
            return;
        }*/

        if (manager.livePlayers.Value <= 0)
            return;

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        GameObject g = Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        g.GetComponent<NetworkObject>().Spawn(true);
    }
}
