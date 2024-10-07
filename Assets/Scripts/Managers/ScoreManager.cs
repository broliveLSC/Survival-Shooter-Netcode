using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Netcode;

public class ScoreManager : NetworkBehaviour
{
    public NetworkVariable<int> score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    [SerializeField] Text text;

    void Update ()
    {
        if (score != null)
        {
            text.text = "Score: " + score.Value;
        }
    }
}
