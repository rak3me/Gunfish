using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkConnection))]
[RequireComponent(typeof(Rigidbody2D))]
public class Gunfish : NetworkBehaviour {

    //#region CONSTANTS
    //public const byte POSITION = 0;
    //public const byte ROTATION = 1;
    //public const byte SCALE = 2;
    //public const byte VELOCITY = 0;
    //#endregion


    #region VARIABLES
    [Header("Input")]
    public float movement; //0 = no movement, 1 = left, 2 = right
    private byte compressedMovement; //0 = no movement, 1 = left, 2 = right
    public float currentJumpCD;
    [Range(0.1f, 5f)] public float maxJumpCD = 1f;
    public bool fire;
    public float currentFireCD;
    [Range(0.1f, 5f)] public float maxFireCD = 1f;

    [Header("Fish Info")]
    public Transform[] children;
    public Vector2[] childrenPositions;
    public float[] childrenRotations;
    public Vector2[] childrenScales;
    public Vector2[] childrenVelocities;
    public byte childCount;
    public Rigidbody2D rb;
    public Gun gun;
    #endregion

    //When the Gunfish is spawned on the server, initialize the children
    //so that messages can be sent to them.
    public override void OnStartServer () {
        childCount = (byte)transform.childCount;
        children = new Transform[childCount];
        childrenPositions = new Vector2[childCount];
        childrenRotations = new float[childCount];
        childrenScales = new Vector2[childCount];
        childrenVelocities = new Vector2[childCount];

        UpdateChildrenInfo();

        rb = GetComponent<Rigidbody2D>();
        gun = GetComponentInChildren<Gun>();

        currentJumpCD = 0f;
        currentFireCD = 0f;
    }

    public void UpdateChildrenInfo () {
        for (int i = 0; i < childCount; i++) {
            if (children[i] == null) {
                children[i] = transform.GetChild(i);
            }

            childrenPositions[i] = children[i].position;
            childrenRotations[i] = children[i].rotation.z;
            childrenScales[i] = children[i].localScale;
            childrenVelocities[i] = children[i].GetComponent<Rigidbody2D>().velocity;
        }
    }

    //Handles messages to be sent between the Server and
    //Client.
    private void Update () {
        if (isClient) {
            if (InputHandler()) {
                Debug.Log("Input detected!");
                NetworkManager.singleton.client.Send(MessageTypes.INPUTMSG, new InputMsg(compressedMovement, fire, gameObject));
            }
        }

        if (isServer) {
            if (currentJumpCD <= 0f) {
                currentJumpCD = 0f;
            } else {
                currentJumpCD -= Time.deltaTime;
            }

            if (currentFireCD <= 0f) {
                currentFireCD = 0f;
            } else {
                currentFireCD -= Time.deltaTime;
            }

            //NetworkServer.SendToAll(MessageTypes.DEBUGLOGMSG, new DebugLogMsg("Updating Transforms!"));
            UpdateChildrenInfo();
            GunfishMsg msg = new GunfishMsg(netId, 
                                            (Vector2)transform.position, 
                                            transform.rotation.z, 
                                            (Vector2)transform.localScale, 
                                            rb.velocity, childrenPositions, 
                                            childrenRotations, childrenScales, 
                                            childrenVelocities);
            NetworkServer.SendToAll(MessageTypes.GUNFISHMSG, msg);
        }
    }

    //public byte DeltaArray (byte type) {
    //    byte[] array = new byte[childCount * 2];

    //    switch (type) {
    //        default:
    //            break;

    //        case POSITION:
    //            for (int i = 0; i < childCount * 2; i += 2) {
    //                array[i] = children[i].position.x - transform.position.x
    //            }
    //            break;

    //        case ROTATION:

    //            break;

    //        case SCALE:
                
    //            break;

    //        case VELOCITY:

    //            break;
    //    }
    //}

    //Checks user input on the Client. Also returns whether
    //or not an input message should be sent to the server.
    public bool InputHandler () {
        bool sendMessage = false;

        if ((movement = Input.GetAxisRaw("Horizontal")) != 0) {
            sendMessage = true;
            if (movement < 0) {
                compressedMovement = 1;
            } else {
                compressedMovement = 2;
            }
        } else {
            compressedMovement = 0;
        }  

        if (fire = Input.GetKeyDown(KeyCode.Space)) {
            sendMessage = true;
        }

        return sendMessage;
    }

    //Called on the server. Applies appropriate forces to the
    //Gunfish. To call this function appropriately, it should
    //be called from the server, after calculating what force
    //and torque should be applied from the server as well.
    public void Move (Vector2 force, float torque) {
        GetComponent<Rigidbody2D>().AddForce(force);
        GetComponent<Rigidbody2D>().AddTorque(torque);

        currentJumpCD = maxJumpCD;
    }

    //Called on the server. Takes the info from the attached Gun
    //component of a child GameObject, and applies a force. If
    //there is no Gun attached, simply will not fire.
    public void Shoot () {
        currentFireCD = maxFireCD;
    }

    //Called on server to check to see if a move command
    //should be issued. MUST be called on the server in case
    //the client is lagging and whatnot. Prevents desync.
    public bool IsGrounded () {
        return true;
    }


}
