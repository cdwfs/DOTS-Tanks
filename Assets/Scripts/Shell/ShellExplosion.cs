using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    private ParticleSystem m_ExplosionParticles;       
    private AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        m_ExplosionParticles = GetComponentInChildren<ParticleSystem>();
        m_ExplosionAudio = GetComponentInChildren<AudioSource>();
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
        foreach (var col in colliders) {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (!rb) {
                continue;
            }
            rb.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = col.GetComponent<TankHealth>();
            if (!targetHealth) {
                continue;
            }
            targetHealth.TakeDamage(CalculateDamage(rb.position));
        }

        // We want to destroy the shell immediately, but that will destroy all child GOs as well
        // (including the particles we want to play), so first de-parent the particles.
        m_ExplosionParticles.transform.parent = null;
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();
        // Particles GO should destroy itself after the particles finish playing
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 toTarget = targetPosition - transform.position;
        float distanceToTarget = toTarget.magnitude;
        float damageScale = Mathf.Clamp01((m_ExplosionRadius - distanceToTarget) / m_ExplosionRadius);
        return damageScale * m_MaxDamage;
    }
}