using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask TankMask;
    public ParticleSystem ExplosionParticles;       
    public AudioSource ExplosionAudio;              
    public float MaxDamage = 100f;                  
    public float ExplosionForce = 1000f;            
    public float MaxLifeTime = 2f;                  
    public float ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        foreach (Collider collider in Physics.OverlapSphere(transform.position, ExplosionRadius, TankMask))
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (!rb) continue;

            rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);

            TankHealth th = collider.GetComponent<TankHealth>();
            if (!th) continue;

            float damage = CalculateDamage(rb.position);

            th.TakeDamage(damage);
        }

        ExplosionParticles.transform.parent = null;
        ExplosionParticles.Play();
        ExplosionAudio.Play();

        Destroy(ExplosionParticles.gameObject, ExplosionParticles.main.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        float explosionDistance = (targetPosition - transform.position).magnitude;
        float relativeDistance = (ExplosionRadius - explosionDistance) / ExplosionRadius;
        float damage = MaxDamage * relativeDistance;

        return Mathf.Max(0f, damage);
    }
}