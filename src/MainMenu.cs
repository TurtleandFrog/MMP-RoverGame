using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LevelSelect()
    {
        LoadScene(SceneName.LevelSelect);
    }

    public void Settings()
    {
        LoadScene(SceneName.SettingsMenu);
    }

    public void Shop()
    {
        LoadScene(SceneName.Shop);
    }

    public void Workshop()
    {
        LoadScene(SceneName.Workshop);
    }  

    public void Credits()
    {
        LoadScene(SceneName.Credits);
    }        

    public void Exit()
    {
        Debug.Log("Exit Application");
        Application.Quit();
    }   

    private void LoadScene(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }     
}
