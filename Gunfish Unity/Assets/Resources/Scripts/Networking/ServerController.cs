using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerController : NetworkBehaviour {

    public override void OnStartServer () {
        ConnectionConfig config = new ConnectionConfig();
        config.DisconnectTimeout = 5000; //If the player times out for 5 seconds, disconnect them

        NetworkServer.RegisterHandler(MessageTypes.INPUTMSG, OnInput);
        NetworkServer.RegisterHandler(MessageTypes.SPAWNMSG, OnSpawnObject);

        SendClientDebugLog("Starting Server!");
    }

    public void SendClientDebugLog (string msg) {
        return;
        NetworkServer.SendToAll(MessageTypes.DEBUGLOGMSG, new DebugLogMsg(msg));
    }

    #region MESSAGE HANDLERS
    private void OnSpawnObject (NetworkMessage netMsg) {
        GameObjectMsg msg = netMsg.ReadMessage<GameObjectMsg>();

        NetworkServer.Spawn(Instantiate<GameObject>(msg.obj));
    }

    private void OnInput (NetworkMessage netMsg) {
        InputMsg msg = netMsg.ReadMessage<InputMsg>();
        GameObject gunfishObj = NetworkServer.FindLocalObject(msg.fish.GetComponent<NetworkIdentity>().netId);
        Gunfish gunfish = gunfishObj.GetComponent<Gunfish>();

        if (gunfish == null) {
            SendClientDebugLog("Error in ServerController - OnInput. GameObject does not have Gunfish component.");
            return;
        }

        byte movement = msg.movement;
        int decompressedMovement = 0;

        if (movement == 1) {
            decompressedMovement = -1;
        } else if (movement == 2) {
            decompressedMovement = 1;
        }

        //Override for movement. If jump is on cooldown, do nothing.
        if (gunfish.currentJumpCD > 0f) {
            decompressedMovement = 0;
        }

        if (decompressedMovement != 0) {
            gunfish.Move(new Vector2(decompressedMovement, 1f).normalized * 200f, -decompressedMovement * 200f * Random.Range(0.5f, 1f));
        }

        if (msg.shoot && gunfish.currentFireCD <= 0) {
            gunfish.Shoot();
        }
    }
    #endregion
}
