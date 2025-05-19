using Google.Protobuf;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    public bool IsConnected = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Update()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in list)
        {
            Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.Id);
            if (handler != null)
                handler.Invoke(_session, packet.Message);
        }
    }

    ServerSession _session = new ServerSession();

    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }

    public void Send(IMessage message)
    {
        if (IsConnected == false)
        {
            return;
        }

        _session.Send(message);
    }

    private void Init()
    {
        //string host = GameManager.Instance.gameServerInfo.ip;
        int port = GameManager.Instance.gameServerInfo.port;

        // DNS (Domain Name System)
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connector connector = new Connector();

        connector.Connect(endPoint,
            () => { return _session; },
            1);
    }
}
