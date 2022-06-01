using Ajuna.NetApi;
using NLog;
using NLog.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{
    [SerializeField]
    private string WebSocketUrl = "ws://127.0.0.1:9944";

    private SubstrateClient _substrateClient;

    private Task _connectTask;

    public Image ConnectImage;

    private void Awake()
    {
        LoggingConfiguration config = new LoggingConfiguration();
        LogManager.Configuration = config;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_connectTask != null && _connectTask.IsCompleted)
        {
            ConnectImage.color = _substrateClient.IsConnected ? Color.green : Color.red;

            // Check and updated current status
            _connectTask = null;
        }
    }

    void OnApplicationQuit()
    {
        _ = DisconnectAsync();
    }

    public void ButtonConnect()
    {
        if (_connectTask != null && !_connectTask.IsCompleted)
        {
            Debug.Log("Connection task, still running!");
            return;
        }

        _connectTask = _substrateClient != null && _substrateClient.IsConnected 
            ? DisconnectAsync() 
            : ConnectAsync();
    }

    private async Task ConnectAsync()
    {
        Debug.Log("Connecting client!");
        _substrateClient = new SubstrateClient(new Uri(WebSocketUrl));
        await _substrateClient.ConnectAsync(false, CancellationToken.None);

    }

    private async Task DisconnectAsync()
    {
        Debug.Log("Disconnecting client!");
        await _substrateClient.CloseAsync();
    }

}
