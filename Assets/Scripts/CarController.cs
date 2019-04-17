using UnityEngine;

[RequireComponent(typeof(CarKinematics))]
public class CarController : MonoBehaviour
{
    private CarKinematics m_CarKinematics;

    private void Awake()
    {
        m_CarKinematics = GetComponent<CarKinematics>();
    }

    public void Update()
    {
        m_CarKinematics.Horizontal = Input.GetAxis("Horizontal");
        m_CarKinematics.Vertical = Input.GetAxis("Vertical");
        m_CarKinematics.LeftBrake = Input.GetButton("Fire1") ? 1.0f : 0.0f;
        m_CarKinematics.RightBrake = Input.GetButton("Fire2") ? 1.0f : 0.0f;
    }
}
