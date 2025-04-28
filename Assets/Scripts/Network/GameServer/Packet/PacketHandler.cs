using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using UnityEngine;

class PacketHandler
{
    public static void S_EnterRoomHandler(PacketSession session, IMessage message)
    {
        S_EnterRoom enterRoom = message as S_EnterRoom;
        ServerSession serverSession = session as ServerSession;
        if (enterRoom == null || serverSession == null)
            return;

        // Handle the S_EnterRoom packet
        Debug.Log("S_EnterRoomHandler called with message: " + enterRoom.ToString());
    }

    public static void S_ReadyCompleteGameHandler(PacketSession session, IMessage message)
    {
        throw new NotImplementedException();
    }
}
