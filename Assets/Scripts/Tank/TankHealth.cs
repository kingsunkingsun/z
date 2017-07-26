using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float StartingHealth = 100f;          
    public Slider Slider;                        
    public Image FillImage;                      
    public Color FullHealthColor = Color.green;  
    public Color ZeroHealthColor = Color.red;    
    public GameObject ExplosionPrefab;
    
    private AudioSource _explosionAudio;          
    private ParticleSystem _explosionParticles;   
    private float _currentHealth;  
    private bool _dead;            


    private void Awake()
    {
        _explosionParticles = Instantiate(ExplosionPrefab).GetComponent<ParticleSystem>();
        _explosionAudio = _explosionParticles.GetComponent<AudioSource>();

        _explosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        _currentHealth = StartingHealth;
        _dead = false;

        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        _currentHealth -= amount;
        SetHealthUI();

        if (_currentHealth <= 0f && !_dead)
            OnDeath();
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        Slider.value = _currentHealth;
        FillImage.color = Color.Lerp(ZeroHealthColor, FullHealthColor, _currentHealth / StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        _dead = true;
        _explosionParticles.transform.position = transform.position;
        _explosionParticles.gameObject.SetActive(true);

        _explosionParticles.Play();
        _explosionAudio.Play();

        gameObject.SetActive(false);
    }
}