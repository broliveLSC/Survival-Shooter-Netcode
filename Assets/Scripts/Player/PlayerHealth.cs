using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;
using Unity.Collections;

public class PlayerHealth : NetworkBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;

    NetworkVariable<FixedString64Bytes> playerID = new NetworkVariable<FixedString64Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] TMP_Text playerIDText;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer && IsLocalPlayer)
            FindObjectOfType<GameManager>().livePlayers.Value += 1;
        else if (IsLocalPlayer)
            AddPlayerServerRpc();

        //if(IsLocalPlayer)
        

        if (IsLocalPlayer)
        {
            damageImage = GameObject.Find("DamageImage").GetComponent<Image>();
            healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();

            FindObjectOfType<PlayerCamera>().SetTarget(transform);

            playerID.Value = GameObject.Find("NameInput").GetComponent<TMP_InputField>().text; //Random.Range(0, 1000);// FindObjectOfType<GameManager>().livePlayers.Value;
        }

    }

    void Update ()
    {
        if(damaged)
        {
            if(IsLocalPlayer && damageImage != null)
                damageImage.color = flashColour;
        }
        else
        {
            if(damageImage != null)
                damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;

        playerIDText.text = "" + playerID.Value;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        if(healthSlider != null)
            healthSlider.value = currentHealth;

        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }

    void Death ()
    {
        isDead = true;

        if (IsServer && IsLocalPlayer)
            FindObjectOfType<GameManager>().livePlayers.Value -= 1;
        else if (IsLocalPlayer)
        {
            RemovePlayerServerRpc();
            FindObjectOfType<PlayerCamera>().NoTarget();
        }

        playerShooting.DisableEffects ();

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;

        if(IsServer)
        {
            Destroy(gameObject, 2);
        }
    }

    public override void OnDestroy()
    {
        if(IsServer && GetComponent<NetworkObject>() != null)
            GetComponent<NetworkObject>()?.Despawn();
    }

    [ServerRpc]
    void AddPlayerServerRpc()
    {
        FindObjectOfType<GameManager>().livePlayers.Value += 1;
    }

    [ServerRpc]
    void RemovePlayerServerRpc()
    {
        FindObjectOfType<GameManager>().livePlayers.Value -= 1;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
