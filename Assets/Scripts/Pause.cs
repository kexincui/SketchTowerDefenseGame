using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject PauseButton;
    public GameObject PausePanel;

    private SketchBoxesManagement SBM;

    void Start()
    {
        SBM = GameObject.Find("GameManagement").GetComponent<SketchBoxesManagement>();
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;

        SBM.isInputEnabled = false;

        PauseButton.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;

        SBM.isInputEnabled = true;

        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1.0f;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("StartMenu");
        Time.timeScale = 1.0f;
    }
}
