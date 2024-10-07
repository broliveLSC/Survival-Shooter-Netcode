using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using TMPro;

public class NetworkMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField hostIPInput;
    [SerializeField] TMP_InputField clientIPInput;
    [SerializeField] TMP_InputField relayCodeInput;

    public void StartLANGame()
    {
        /*Debug.Log("Before reading <" + NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address + ">");
        Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);
        Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port);*/

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(hostIPInput.text, (ushort)7777);

        /*Debug.Log("after reading <" + NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address + ">");
        Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);
        Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port);*/

        NetworkManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }

    public void JoinLANGame()
    {
        //NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = clientIPInput.text;
        //NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(clientIPInput.text, 7777);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(clientIPInput.text, (ushort)7777);

        NetworkManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }

    public void JoinRelay()
    {
        FindObjectOfType<RelayHandler>().JoinRelay(relayCodeInput.text);
        gameObject.SetActive(false);
    }
}
