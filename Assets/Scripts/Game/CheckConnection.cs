using System;
using System.Collections;
using Game.Durak;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;
using System.Threading.Tasks;
using System.Threading;

[Serializable]
public class CheckConnection
{
    [SerializeField] private TextMeshProUGUI _pingText;
    
    [SerializeField] private int _pingLimit;
    
    private MonoBehaviour _monoBehaviour;

    private bool _isPinging;

    private int _connectionAttempt = 0;

    private WebSocket _webSocket;

    private enum PingStatus
    {
        Success,
        TimeOut
    }

    private PingStatus _pingStatus;


    public CheckConnection(MonoBehaviour monoBehaviour, WebSocket webSocket)
    {
        _monoBehaviour = monoBehaviour;
        _webSocket = webSocket;
    }

    public CheckConnection Clone(CheckConnection original)
    {
        return new CheckConnection(_monoBehaviour, _webSocket)
        {
            _pingLimit = original._pingLimit,
            _pingText = original._pingText,
        };
    }

    public void PingHost(Action closeSocket) => _monoBehaviour.StartCoroutine(HostPinging(closeSocket));
    public async void PingHostAsync(Action closeSocket)
    {
        await HostPingingAsync(closeSocket);
    }
    

    public void CanselPinging()
    {
        _isPinging = false;
    }

    private async Task HostPingingAsync(Action closeSocket)
    {
        _isPinging = true;

        bool isAlive = true;
        
        double _sendingTime;
        double _receivingTime;
    
        double ping;

        TimeSpan delay = TimeSpan.FromSeconds(1);

        while (_isPinging)
        {
            await Task.Delay(delay);

            _sendingTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            
            await Task.Run(() => isAlive = _webSocket.Ping());

            Debug.Log(isAlive);
            
            _receivingTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

            ping = _receivingTime - _sendingTime;
            
            if (isAlive)
                _pingStatus = PingStatus.Success;
            else
                _pingStatus = PingStatus.TimeOut;

            PingReplyStatusHandler(_pingStatus, ping, closeSocket);

            Task.Yield();
        }

        _isPinging = false;

    }
    
    private IEnumerator HostPinging(Action closeSocket)
    {
        _isPinging = true;

        bool isAlive;
        
        double _sendingTime;
        double _receivingTime;
    
        double ping;
                
        YieldInstruction delay = new WaitForSeconds(1);

        while (_isPinging)
        {
            yield return delay;

            _sendingTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            
            yield return isAlive = _webSocket.Ping();

            Debug.Log(isAlive);
            
            _receivingTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

            ping = _receivingTime - _sendingTime;
            
            if (isAlive)
                _pingStatus = PingStatus.Success;
            else
                _pingStatus = PingStatus.TimeOut;

            PingReplyStatusHandler(_pingStatus, ping, closeSocket);
            
            yield return null;
        }

        _isPinging = false;

    }
    
    private void PingReplyStatusHandler(PingStatus status, double ping, Action callback)
    {
        switch (status)
        {
            case PingStatus.Success :
                Debug.Log($"Ping: {ping:F1}");
                ShowPing(ping);
                break;
            case PingStatus.TimeOut :
                Debug.Log($"Ping: timeout");
                ShowPing(0);
                callback.Invoke();
                _isPinging = false;
                break;
        }
    }

    private void ShowPing(double ping) => _pingText.text = $"{ping:F1}";
}