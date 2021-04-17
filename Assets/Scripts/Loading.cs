using System.Collections;
using MLAPI;
using MLAPI.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : NetworkBehaviour
{
	public Slider bar;
	
	
	void Start()
	{
		if(IsServer)
			NetworkSceneManager.SwitchScene("GamePlay");
		else
		{
			if(!IsClient)
				StartCoroutine(LoadYourAsyncScene());
		}
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