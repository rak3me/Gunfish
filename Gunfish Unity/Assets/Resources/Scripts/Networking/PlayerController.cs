using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    [Header("Input")]
    public int moveX;
    private byte compressedMoveX;
    public bool shooting;

    [Header("Player Info")]
    public GameObject localPlayerConnectionObj;
    public GameObject gunfish;

    public override void OnStartLocalPlayer () {
        NetworkManager.singleton.client.RegisterHandler(MessageTypes.DEBUGLOGMSG, OnDebugLog);
        NetworkManager.singleton.client.RegisterHandler(MessageTypes.GUNFISHMSG, OnGunfish);
        NetworkManager.singleton.client.RegisterHandler(MessageTypes.SPAWNMSG, OnSpawnGameObject);
        Debug.Log("ConnId: " + connectionToServer.connectionId);
        return;
        gunfish = GameObject.FindGameObjectsWithTag("Player")[connectionToServer.connectionId];
        NetworkManager.singleton.client.Send(MessageTypes.SPAWNMSG, new GameObjectMsg(gunfish));

        GameObject.FindWithTag("MainCamera").GetComponent<Camera2DFollow>().target = gunfish.transform;
    }

    #region MESSAGE HANDLERS

    private void OnSpawnGameObject (NetworkMessage netMsg) {
        NetIdMsg msg = netMsg.ReadMessage<NetIdMsg>();


    }

    private void OnDebugLog (NetworkMessage netMsg) {
        DebugLogMsg msg = netMsg.ReadMessage<DebugLogMsg>();

        Debug.Log(msg.log);
    }

    private void OnGunfish (NetworkMessage netMsg) {
        GunfishMsg msg = netMsg.ReadMessage<GunfishMsg>();

        Transform fish = NetworkServer.FindLocalObject(msg.netId).transform;

        byte childCount = (byte)fish.childCount;

        fish.position = msg.startPosition;
        fish.eulerAngles = new Vector3(0f, 0f, msg.startRotation);
        fish.localScale = msg.startScale;
        fish.GetComponent<Rigidbody2D>().velocity = msg.startVelocity;

        for (int i = 0; i < childCount; i++) {
            Transform child = fish.GetChild(i);

            child.position = msg.positions[i];
            child.eulerAngles = new Vector3(0f, 0f, msg.rotations[i]);
            child.localScale = msg.scales[i];
            child.GetComponent<Rigidbody2D>().velocity = msg.velocities[i];
        }
    }

    #endregion
}
