using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour
{
   public GameObject pausedMenu;

   private void Awake()
   {
      Resume();
   }

   public void Resume()
   {
      Time.timeScale = 1f;
      pausedMenu.SetActive(false);
   }

   public void Pause()
   {
      pausedMenu.SetActive(true);
      Time.timeScale = 0f;
   }

   public void Restart()
   {
      SceneManager.LoadScene("Loading");
   }

   public void MainMenu()
   {
      SceneManager.LoadScene("MainMenu");
   }
}
