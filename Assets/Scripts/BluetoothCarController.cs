using UnityEngine;

[RequireComponent(typeof(CarKinematics))]
public class BluetoothCarController : MonoBehaviour
{ 
    private CarKinematics m_CarKinematics;

    private void Awake()
    {
        m_CarKinematics = GetComponent<CarKinematics>();
    }

    public void Update()
    {
        if (BluetoothManager.Instance.IsConnected)
        {
            BluetoothRead();
            UpdateInput();
            BluetoothWrite();
        }
    }

    private void BluetoothRead()
    {

    }

    private void BluetoothWrite()
    {

    }

    private void UpdateInput()
    {

    }
}