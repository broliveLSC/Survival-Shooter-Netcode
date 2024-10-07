using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class GameManager : NetworkBehaviour
{
    public NetworkVariable<int> livePlayers = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private Dictionary<int, PlayerHealth> _playerIdToPlayer = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void OnNetworkSpawn()
    {
        livePlayers.OnValueChanged += CheckForGameOver;
    }

    void CheckForGameOver(int prev, int next)
    {
        if (next == 0)
        {
            FindObjectOfType<GameOverManager>().GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current players: " + livePlayers.Value);
    }


    public int AddPlayer(PlayerHealth player)
    {
        var playerId = Random.Range(0, 1000);
        Debug.Log("[NetworkGameManager] Adding player with id " + playerId + "to dictionary");

        _playerIdToPlayer.Add(playerId, player);
        Debug.Log("[NetworkGameManager] Total players after adding player " + _playerIdToPlayer.Count);
        return playerId;
    }
}
