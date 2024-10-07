using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            // if pause menu is open, pressing pause button means we want to unpause
            if(SceneManager.GetSceneByName("Pause").isLoaded)
            {
                UnpauseGame();
            }
            // if pause menu is not open, pressing pause button means we want to pause
            else
            {
                PauseGame();
            }
        }
    }

    [ContextMenu("Pause")]
    public void PauseGame()
    {
        SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
    }

    public void UnpauseGame()
    {
        SceneManager.UnloadSceneAsync("Pause");
    }
}
