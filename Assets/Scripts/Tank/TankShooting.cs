using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int PlayerNumber = 1;
    public Rigidbody ShellPrefab;
    public Transform FireTransform;
    public Slider AimSlider;
    public AudioSource ShootingAudio;
    public AudioClip ChargingClip;
    public AudioClip FireClip;
    public float MinLaunchForce = 15f;
    public float MaxLaunchForce = 30f;
    public float MaxChargeTime = 0.75f;


    private string _fireButton;
    private float _currentLaunchForce;
    private float _chargeSpeed;
    private bool _fired;


    private void OnEnable()
    {
        _currentLaunchForce = MinLaunchForce;
        AimSlider.value = MinLaunchForce;
    }


    private void Start()
    {
        _fireButton = "Fire" + PlayerNumber;

        _chargeSpeed = (MaxLaunchForce - MinLaunchForce) / MaxChargeTime;
    }


    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        AimSlider.value = MinLaunchForce;

        if (_currentLaunchForce >= MaxLaunchForce && !_fired)
        {
            //at max charge, not yet fired
            _currentLaunchForce = MaxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(_fireButton))
        {
            //have we pressed the fire for the first time?
            _fired = false;
            _currentLaunchForce = MinLaunchForce;
            ShootingAudio.clip = ChargingClip;
            ShootingAudio.Play();
        }
        else if (Input.GetButton(_fireButton) && !_fired)
        {
            //keep pressing, not yet fired
            _currentLaunchForce += _chargeSpeed * Time.deltaTime;
            AimSlider.value = _currentLaunchForce;
        }
        else if (Input.GetButtonUp(_fireButton) && !_fired)
        {
            //released the button, not yet fired
            Fire();
        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        _fired = true;
        Rigidbody shellInstance = Instantiate(ShellPrefab, FireTransform.position, FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = FireTransform.forward * _currentLaunchForce;

        ShootingAudio.clip = FireClip;
        ShootingAudio.Play();

        _currentLaunchForce = MinLaunchForce;

    }
}