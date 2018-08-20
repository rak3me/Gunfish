using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageTypes {
    public static short DEBUGLOGMSG = MsgType.Highest + 1;

    public static short INPUTMSG = MsgType.Highest + 2;
    public static short GUNFISHMSG = MsgType.Highest + 3;

    public static short SPAWNMSG = MsgType.Highest + 4;
}
