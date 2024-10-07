using UnityEngine;
using Unity.Netcode;

public class GameOverManager : MonoBehaviour
{
    PlayerHealth playerHealth;
	public float restartDelay = 5f;
    bool doGameOver = false;
    Animator anim;
	float restartTimer;

    GameManager gm;

    void Awake()
    {
        anim = GetComponent<Animator>();
        gm = FindObjectOfType<GameManager>();
    }


    void Update()
    {
        if(doGameOver)
        {
            restartTimer += Time.deltaTime;

            if (restartTimer >= restartDelay)
            {
                NetworkManager.Singleton.Shutdown();
                Application.LoadLevel(Application.loadedLevel);
            }
        }
        /*if (gm.livePlayers.Value <= 0)
        {
            anim.SetTrigger("GameOver");

			restartTimer += Time.deltaTime;

			if (restartTimer >= restartDelay) {
				Application.LoadLevel(Application.loadedLevel);
			}
        }*/
    }

    public void GameOver()
    {
        anim.SetTrigger("GameOver");

        doGameOver = true;
    }
}
