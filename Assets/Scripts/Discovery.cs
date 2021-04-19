using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Discovery : NetworkDiscovery
{
	
	private float timeout = 5f;

	private Dictionary<IPAddress, float> lanAddresses = new Dictionary<IPAddress, float>();
	
	private void Awake()
	{
		base.Initialize();
		base.StartAsClient();
		StartCoroutine(CleanupExpiredEntries());
	}

	public void StartBroadcast()
	{
		Debug.Log("Boradcast Started");
		StopBroadcast();
		base.Initialize();
		base.StartAsServer();
	}
	
	private IEnumerator CleanupExpiredEntries()
	{
		while(true)
		{
			bool changed = false;

			var keys = lanAddresses.Keys.ToList();
			foreach (var key in keys)
			{
				if(lanAddresses[key] <= Time.time)
				{
					lanAddresses.Remove(key);
					changed = true;
				}
			}
			if(changed)
				UpdateMatchInfos();

			yield return new WaitForSeconds(timeout);
		}
	}

	public override void OnReceivedBroadcast(string fromAddress, string data)
	{
		Debug.Log("Boradcast Rec");
		base.OnReceivedBroadcast(fromAddress, data);

		IPAddress info = new IPAddress(fromAddress, data);

		if(lanAddresses.ContainsKey(info) == false)
		{
			lanAddresses.Add(info, Time.time + timeout);
			UpdateMatchInfos();
		}
		else
		{
			lanAddresses[info] = Time.time + timeout;
		}
	}

	private void UpdateMatchInfos()
	{
		//GameListController.AddLanMatches(lanAddresses.Keys.ToList());
		foreach (var x in lanAddresses.Keys.ToList())
		{
			Debug.Log($"Ip: {x.ipAddress} -> Port: {x.port} -> Name: {x.name}");
		}
	}
}