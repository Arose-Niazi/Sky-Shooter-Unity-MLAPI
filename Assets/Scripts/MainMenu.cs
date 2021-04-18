using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject ipPanel;
    public InputField ipField;
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
        //SceneManager.LoadScene("Hosting");
    }
    
    public void Join()
    {
        ipPanel.SetActive(true);
    }

    public void Connect()
    {
        if(!ValidateIPv4(ipField.text)) return;
        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = ipField.text;
        NetworkManager.Singleton.StartClient();
    }

    public void Back()
    {
        ipPanel.SetActive(false);
    }
    
    public bool ValidateIPv4(string ipString)
    {
        if (string.IsNullOrWhiteSpace(ipString))
        {
            return false;
        }

        string[] splitValues = ipString.Split('.');
        if (splitValues.Length != 4)
        {
            return false;
        }

        byte tempForParsing;

        return splitValues.All(r => byte.TryParse(r, out tempForParsing));
    }


}
