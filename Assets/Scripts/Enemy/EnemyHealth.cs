using UnityEngine;
using Unity.Netcode;
public class EnemyHealth : NetworkBehaviour
{
    public int startingHealth = 100;
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        currentHealth.Value = startingHealth;
    }

    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }

        if (currentHealth.Value <= 0 && !isDead)
        {
            Death();
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        if(IsServer)
            currentHealth.Value -= amount;
        else
        {
            TakeDamageServerRpc(amount, hitPoint);
        }
            
        //hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        /*if(currentHealth.Value <= 0)
        {
            DeathServerRpc ();
        }*/
    }

    [ServerRpc(RequireOwnership =false)]
    void TakeDamageServerRpc(int amount, Vector3 hitPoint)
    {
        currentHealth.Value -= amount;
        ShowHitEffectClientRpc(hitPoint);
    }

    [ClientRpc]
    void ShowHitEffectClientRpc(Vector3 hitPoint)
    {
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();
    }

    void Death ()
    {
        if (IsServer)
            FindObjectOfType<ScoreManager>().score.Value += scoreValue;
        /*else
            IncreaseScoreServerRpc(scoreValue);*/

        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }

    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;

        Destroy (gameObject, 2f);
        
    }

    public override void OnDestroy()
    {
        if (IsServer && GetComponent<NetworkObject>() != null)
            GetComponent<NetworkObject>()?.Despawn();
    }
}
