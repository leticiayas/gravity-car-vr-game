using UnityEngine;

public class CarKinematics : MonoBehaviour
{
    public float Horizontal { get; set; }
    public float Vertical { get; set; }
    public float LeftBrake { get; set; }
    public float RightBrake { get; set; }

    [SerializeField]
    private Transform m_CenterOfGravity;

    [SerializeField]
    private WheelCollider m_WheelColliderFL, m_WheelColliderFR;

    [SerializeField]
    private WheelCollider m_WheelColliderRL, m_WheelColliderRR;

    [SerializeField]
    private Transform m_WheelFL, m_WheelFR;

    [SerializeField]
    private Transform m_WheelRL, m_WheelRR;
    
    [SerializeField]
    private Transform m_PivotFL, m_PivotFR;

    [SerializeField]
    private Transform m_FrontAxle;

    [SerializeField]
    private float m_MaxSteeringAngle = 30.0f;

    [SerializeField]
    private float m_MotorForce = 50.0f;

    [SerializeField]
    private float m_BrakeTorque = 100.0f;

    public enum DriveMode {  Front, Rear, All, None };
    public DriveMode m_DriveMode = DriveMode.All;

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        m_Rigidbody.centerOfMass = m_CenterOfGravity.localPosition;
    }

    public void FixedUpdate()
    {
        Steer();
        Accelerate();
        Brake();
        UpdateWheelPoses();
    }

    public void Steer()
    {
        float steeringAngle =  m_MaxSteeringAngle * Horizontal;

        m_WheelColliderFL.steerAngle = steeringAngle;
        m_WheelColliderFR.steerAngle = steeringAngle;

        m_FrontAxle.localRotation = Quaternion.Euler(Vector3.up * steeringAngle);

        m_WheelFL.position = m_PivotFL.position;
        m_WheelFL.position = m_PivotFR.position;
    }

    private void Accelerate()
    {
        if (m_DriveMode == DriveMode.Front || m_DriveMode == DriveMode.All)
        {
            m_WheelColliderFL.motorTorque = Vertical * m_MotorForce;
            m_WheelColliderFR.motorTorque = Vertical * m_MotorForce;
        }
        else
        {
            m_WheelColliderFL.motorTorque = 0;
            m_WheelColliderFR.motorTorque = 0;
        }

        if (m_DriveMode == DriveMode.Rear || m_DriveMode == DriveMode.All)
        {
            m_WheelColliderRL.motorTorque = Vertical * m_MotorForce;
            m_WheelColliderRR.motorTorque = Vertical * m_MotorForce;
        }
        else
        {
            m_WheelColliderRL.motorTorque = 0;
            m_WheelColliderRR.motorTorque = 0;
        }
    }

    private void Brake()
    {
        m_WheelColliderRL.brakeTorque = m_BrakeTorque * LeftBrake;
        m_WheelColliderRR.brakeTorque = m_BrakeTorque * RightBrake;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(m_WheelColliderFL, m_WheelFL, m_PivotFL);
        UpdateWheelPose(m_WheelColliderFR, m_WheelFR, m_PivotFR);
        UpdateWheelPose(m_WheelColliderRL, m_WheelRL, null);
        UpdateWheelPose(m_WheelColliderRR, m_WheelRR, null);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheel, Transform pivot)
    {
        Vector3 position = wheel.position;
        Quaternion rotation = wheel.rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        rotation = rotation * Quaternion.Euler(new Vector3(0, 0, 90));
        wheel.rotation = rotation;

        if (pivot)
        {
            wheel.position = pivot.position;
        }
        else
        {
            wheel.position = position;
        }
    }
}
