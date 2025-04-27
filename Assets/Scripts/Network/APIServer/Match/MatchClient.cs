using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class MatchClient : MonoBehaviour
{
    public void Start()
    {
        int userId = UserDataManager.Instance._userInfo.userId;

        // 우선 캐릭터 ID를 하드코딩 
        int characterId = GameManager.Instance.currentCharacterId;
        characterId = 1;

        string mode = GamemodeHelper.GetGamemodeName(GameManager.Instance.currentGamemode);

        // 매칭 처리
        StartMatch(userId, characterId, mode);
    }

    private ClientWebSocket _webSocket;
    private CancellationTokenSource _cts;

    private async void StartMatch(long userId, int characterId, string mode)
    {
        _webSocket = new ClientWebSocket();
        _cts = new CancellationTokenSource();

        try
        {
            await _webSocket.ConnectAsync(new Uri("ws://localhost:5001/ws/match"), _cts.Token);

#if UNITY_EDITOR
            Debug.Log("WebSocket 연결됨");
#endif

            var request = new MatchRequest
            {
                userId = userId,
                characterId = characterId,
                skinId = 1, // 하드코딩된 스킨 ID
                mode = mode
            };
            var json = JsonUtility.ToJson(request);

            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, _cts.Token);

            ReceiveLoop(); // 매칭 응답 기다리기
        }
        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError($"❌ WebSocket 오류: {ex.Message}");
#endif
        }
    }

    private async void ReceiveLoop()
    {
        var buffer = new byte[1024 * 4];

#if UNITY_EDITOR
        Debug.Log("매칭 응답 대기 중...");
#endif
        while (_webSocket.State == WebSocketState.Open)
        {
            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
            var jsonResponse = Encoding.UTF8.GetString(buffer, 0, result.Count);

            if (_webSocket != null && _webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Match complete", CancellationToken.None);
#if UNITY_EDITOR
                Debug.Log("✅ WebSocket 연결 종료됨");
#endif
            }
            var response = JsonUtility.FromJson<MatchResponse>(jsonResponse);

#if UNITY_EDITOR
            Debug.Log("매칭 응답 수신됨: " + jsonResponse);
#endif

            // 현재는 데스매치 맵으로 이동하도록 하드코딩
            GameManager.Instance.JoinGame(response.roomId, response.password, response.ip, response.port);
        }
    }

    private async void OnApplicationQuit()
    {
        _cts?.Cancel();

        if (_webSocket != null && _webSocket.State == WebSocketState.Open)
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client exit", CancellationToken.None);
        }

        _webSocket?.Dispose();
    }
}
