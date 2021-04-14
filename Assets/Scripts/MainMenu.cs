using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Loading");
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        SceneManager.LoadScene("Hosting");
    }
    
    public void Client()
    {
        NetworkManager.Singleton.StartClient();
        if(NetworkManager.Singleton.IsListening)
            SceneManager.LoadScene("Hosting");
    }

}
