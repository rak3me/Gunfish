using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DebugLogMsg : MessageBase {
    public string log;

    public DebugLogMsg() { }

    public DebugLogMsg(string _log) {
        log = _log;
    }
}

public class NetIdMsg : MessageBase { 
    public NetworkInstanceId netId;

    public NetIdMsg () { }

    public NetIdMsg (NetworkInstanceId netId) {
        this.netId = netId;
    }
}

public class GameObjectMsg : MessageBase { 
    public GameObject obj;

    public GameObjectMsg () { }

    public GameObjectMsg (GameObject obj) {
        this.obj = obj;
    }
}

public class InputMsg : MessageBase {
    //0 = not moving, 1 = left, 2 = right;
    public byte movement;
    public bool shoot;

    public GameObject fish;

    public InputMsg() { }

    public InputMsg(byte move, bool fire, GameObject gunfish) {
        movement = move;
        shoot = fire;

        fish = gunfish;
    }
}

public class GunfishMsg : MessageBase {
    public NetworkInstanceId netId;

    public Vector2 startPosition;
    public float startRotation;
    public Vector2 startScale;
    public Vector2 startVelocity;

    public Vector2[] positions;
    public float[] rotations;
    public Vector2[] scales;
    public Vector2[] velocities;

    public GunfishMsg () { }

    public GunfishMsg (NetworkInstanceId netId, Vector2 startPosition, float startRotation, Vector2 startScale, Vector2 startVelocity, Vector2[] positions, float[] rotations, Vector2[] scales, Vector2[] velocities) {
        this.netId = netId;

        this.startPosition = startPosition;
        this.startRotation = startRotation;
        this.startScale = startScale;
        this.startVelocity = startVelocity;

        this.positions = positions;
        this.rotations = rotations;
        this.scales = scales;
        this.velocities = velocities;
    }
}

//public class GunfishMsg : MessageBase {
//    NetworkIdentity netId;

//    Vector3 startPosition;
//    Vector3 startRotation;
//    Vector3 startScale;
//    Vector2 startVelocity;

//    byte[] childPositions;
//    byte[] childRotations;
//    byte[] childScales;
//    byte[] childVelocities;

//    public GunfishMsg () { }

//    public GunfishMsg (NetworkIdentity networkId, Vector3 startPos, byte[] childPos, Vector3 startRot, byte[] childRot, Vector3 startScl, byte[] childScl, Vector2 startVel, byte[] childVel) {
//        netId = networkId;

//        startPosition = startPos;
//        startRotation = startRot;
//        startScale = startScl;
//        startVelocity = startVel;

//        childPositions = childPos;
//        childRotations = childRot;
//        childScales = childScl;
//        childVelocities = childVel;
//    }
//}