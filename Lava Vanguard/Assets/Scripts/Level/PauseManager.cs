using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Async;

public class PauseMamager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject deathMenuPanel;
    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (deathMenuPanel.activeSelf){
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Pause(){
        isPaused = true;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = 0f;
    }

    public void Resume(){
        isPaused = false;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = 1f;
    }

    public void Restart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void SwitchWeaponPanel(){
        UIGameManager.Instance.Switch<WeaponPanel>();
    }
}
