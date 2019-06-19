using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MaxChargeTime = 0.75f;

    private Slider m_AimSlider;
    private Transform m_FireTransform;
    private float m_MinLaunchForce = 0.0f;
    private float m_MaxLaunchForce = 0.0f;
    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;                

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        if (m_AimSlider != null) {
            m_AimSlider.value = m_MinLaunchForce;
        }
    }


    private void Start()
    {
        Slider[] sliders = GetComponentsInChildren<Slider>();
        foreach (Slider s in sliders) {
            if (s.name == "AimSlider") {
                m_AimSlider = s;
                break;
            }
        }
        Debug.Assert(m_AimSlider != null);
        m_MinLaunchForce = m_AimSlider.minValue;
        m_MaxLaunchForce = m_AimSlider.maxValue;
        m_AimSlider.value = m_AimSlider.minValue;

        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms) {
            if (t.gameObject.name == "FireTransform") {
                m_FireTransform = t;
                break;
            }
        }
        Debug.Assert(m_FireTransform != null);

        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_AimSlider.minValue;
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired) {
            // at max charge, not fired
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        } else if (Input.GetButtonDown(m_FireButton)) {
            // just pressed fire
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        } else if (Input.GetButton(m_FireButton) && !m_Fired) {
            // charging
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        } else if (Input.GetButtonUp(m_FireButton) && !m_Fired) {
            // charging -> fire
            Fire();
        }

    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;
        Rigidbody shellInstance = (Rigidbody)Instantiate(m_Shell, m_FireTransform);
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}