using ArduinoBluetoothAPI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BluetoothManager : MonoBehaviour
{
    public static BluetoothManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private string m_DeviceName = "HC-05";

    [SerializeField]
    private Text m_DebugText;

    private BluetoothHelper m_BluetoothHelper;

    public string ReceiveMessage { get; private set; }

    public void Start()
    {
        try
        {
            m_BluetoothHelper = BluetoothHelper.GetInstance(m_DeviceName);
            m_BluetoothHelper.OnConnected += OnConnected;
            m_BluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            m_BluetoothHelper.OnDataReceived += OnMessageReceived;
            m_BluetoothHelper.setTerminatorBasedStream("\n");

            if (m_BluetoothHelper.isDeviceFound())
            {
                ShowDebug("Device found!");
            }
            else
            {
                ShowDebug("Device not found!");
            }
        }
        catch (Exception ex)
        {
            ShowDebug(ex.Message);
        }
    }

    public void ShowDebug(string message)
    {
        if (m_DebugText)
        {
            m_DebugText.text = message;
        }

        Debug.Log(message);
    }

    private void OnMessageReceived()
    {
        ReceiveMessage = m_BluetoothHelper.Read();
        ShowDebug(ReceiveMessage);
    }

    private void OnConnectionFailed()
    {
        ShowDebug("Bluetooth failed");
    }

    private void OnConnected()
    {
        ShowDebug("Bluetooth connected");
        try
        {
            m_BluetoothHelper.StartListening();
        }
        catch (Exception ex)
        {
            ShowDebug(ex.Message);
        }
    }

    public void Send(string message)
    {
        if (m_BluetoothHelper != null && m_BluetoothHelper.isConnected())
        {
            m_BluetoothHelper.SendData(message);
        }
    }

    public void Connect()
    {
        if (m_BluetoothHelper != null && !m_BluetoothHelper.isConnected())
        {
            m_BluetoothHelper.isConnected();
        }
    }

    public bool IsConnected => m_BluetoothHelper != null && m_BluetoothHelper.isConnected();

    public void Disconnect()
    {
        if (m_BluetoothHelper != null && m_BluetoothHelper.isConnected())
        {
            m_BluetoothHelper.StopListening();
        }
    }

    public void OnDestroy()
    {
        if (m_BluetoothHelper != null)
        {
            m_BluetoothHelper.StopListening();
        }
    }
}
