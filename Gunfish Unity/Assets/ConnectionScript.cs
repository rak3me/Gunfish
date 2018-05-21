﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionScript : NetworkBehaviour {

	[SerializeField] private GameObject gunfishPrefab;

	// Use this for initialization
	override public void OnStartLocalPlayer () {
		//Resources.Load ("Prefabs/Gunfish");
		CmdSpawnFish ();
	}

	[Command] private void CmdSpawnFish () {
		GameObject go = Instantiate (gunfishPrefab, transform.position, Quaternion.identity) as GameObject;
		NetworkServer.SpawnWithClientAuthority (go, connectionToClient);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}