using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public Gradient m_HealthGradient;
    public GameObject m_ExplosionPrefab;

    private float m_StartingHealth;
    private Slider m_HealthSlider = null;
    private Image m_HealthSliderFillImage = null;
    private AudioSource m_ExplosionAudio;          
    private ParticleSystem m_ExplosionParticles;   
    private float m_CurrentHealth;  
    private bool m_Dead;            

    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
        m_ExplosionParticles.gameObject.SetActive(false);

        Slider[] sliders = GetComponentsInChildren<Slider>();
        foreach (Slider s in sliders) {
            if (s.name == "HealthSlider") {
                m_HealthSlider = s;
                break;
            }
        }
        Debug.Assert(m_HealthSlider != null);
        m_StartingHealth = m_HealthSlider.maxValue;

        Image[] images = m_HealthSlider.GetComponentsInChildren<Image>();
        foreach (Image img in images) {
            if (img.name == "Fill") {
                m_HealthSliderFillImage = img;
                break;
            }
        }
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        m_CurrentHealth -= amount;
        SetHealthUI();
        if (m_CurrentHealth <= 0.0f && !m_Dead) {
            OnDeath();
        }
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_HealthSlider.value = m_CurrentHealth;
        m_HealthSliderFillImage.color = m_HealthGradient.Evaluate(m_CurrentHealth / m_StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();
        gameObject.SetActive(false);
    }
}