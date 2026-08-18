using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    protected SetButton setButton;
    protected bool isClicking;

    // Start is called before the first frame update
    void Start()
    {
        setButton = FindObjectOfType<SetButton>();
    }

    // Update is called once per frame
    void Update()
    {
        ClickRP();
        //°²×¿¶Ë
        /*if (!isClicking && setButton.Pressed)
        {
            isClicking = true;
            ClickRP();    
        }
        if (isClicking && !setButton.Pressed)
        {
            isClicking = false; 
        }  */
    }

    private void ClickRP()
    {
        //PC¶Ë
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioController.Instance.PlayClick();
            GameController.Instance.GetComponent<AudioSource>().Stop();

            if (GameIsPaused)
            {
                Resume();
                GameController.Instance.GetComponent<AudioSource>().Play();
            }
            else
            {
                Pause();
            }
        }

        //°²×¿¶Ë
        /*if (Input.GetMouseButtonDown(0))
        {
            AudioController.Instance.PlayClick();
            GameController.Instance.GetComponent<AudioSource>().Stop();

            if (GameIsPaused)
            {
                Resume();
                GameController.Instance.GetComponent<AudioSource>().Play();
            }
            else
            {
                Pause();
            }
        }*/
    }


    public void Resume()
    {
        AudioController.Instance.PlayClick();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        GameController.Instance.GetComponent<AudioSource>().Play();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void MainMenu()
    {
        GameIsPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        AudioController.Instance.PlayClick();
        Application.Quit();
    }
}
