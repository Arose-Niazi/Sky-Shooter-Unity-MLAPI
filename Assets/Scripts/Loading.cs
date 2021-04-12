using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
	public Slider bar;
	void Start()
	{
		StartCoroutine(LoadYourAsyncScene());
	}
    
	IEnumerator  LoadYourAsyncScene()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GamePlay");
        
		while (asyncLoad.progress < 1)
		{
			bar.value = asyncLoad.progress;
			yield return new WaitForEndOfFrame();
		}
	}
}