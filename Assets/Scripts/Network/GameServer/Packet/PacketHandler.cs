using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using UnityEngine;
using UnityEngine.UIElements;

class PacketHandler
{
    public static void S_EnterRoomHandler(PacketSession session, IMessage message)
    {
        S_EnterRoom enterRoom = message as S_EnterRoom;
        ServerSession serverSession = session as ServerSession;
        if (enterRoom == null || serverSession == null)
            return;

        if(enterRoom.EnterResult != S_EnterRoom.Types.EnterResult.Success)
        {
            Debug.LogError("Failed to enter room: " + enterRoom.EnterResult.ToString());
            return;
        }

        // Handle the S_EnterRoom packet
        Debug.Log("S_EnterRoomHandler called with message: " + enterRoom.ToString());

        bool enterSuccess = true;

        PlayerFactory playerFactory = PlayerFactory.Instance;
        if (playerFactory != null)
        {
            foreach (var info in enterRoom.PlayerInfos)
            {
                GameObject player = playerFactory.CreatePlayer(
                    info.UserId,
                    info.CharacterId,
                    info.SkinId,
                    info.Nickname,
                    new Vector2 { x = info.SpawnPos.X, y = info.SpawnPos.Y });
                if (player == null)
                {
                    enterSuccess = false;    
                }
            }
        }

        // 세팅 완료 여부에 따라 다른 패킷 전송
        C_ReadyCompleteGame readyCompletePacket = new C_ReadyCompleteGame()
        {
            UserId = enterSuccess ? UserDataManager.Instance._userInfo.userId : -1
        };
        NetworkManager.Instance.Send(readyCompletePacket);
    }

    public static void S_ReadyCompleteGameHandler(PacketSession session, IMessage message)
    {
        S_ReadyCompleteGame enterRoom = message as S_ReadyCompleteGame;
        ServerSession serverSession = session as ServerSession;
        if (enterRoom == null || serverSession == null)
            return;

        Debug.Log("S_ReadyCompleteGameHandler called with message: " + enterRoom.ToString());

        MatchManager.Instance.StartGame();
    }

    public static void S_PositionUpdateHandler(PacketSession session, IMessage message)
    {
        S_PositionUpdate positionUpdate = message as S_PositionUpdate;
        ServerSession serverSession = session as ServerSession;
        if (positionUpdate == null || serverSession == null)
            return;

        Debug.Log("S_PositionUpdateHandler called with message: " + positionUpdate.ToString());

        MatchManager.Instance.UpdatePosition(positionUpdate.PlayerPosUpdates);
    }
}
