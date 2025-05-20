using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GunController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private GunData data;  // Reference to the GunData scriptable object containing gun stats

    [Header("Shot Origins")]
    [SerializeField] private Camera mainCam;  // Camera to use for shooting direction
    [SerializeField] private Transform muzzle;  // Transform to define where the bullet will shoot from
    [SerializeField] private Transform sight;  // Transform for sight positioning (for aiming)

    [Header("Operational State")]
    [SerializeField] private bool canFire;  // Flag to check if the gun can fire
    [SerializeField] private bool triggerDown;  // Flag for whether the trigger is being pulled
    [SerializeField] private bool reloading;  // Flag to check if the gun is reloading

    [Header("Ammo Info")]
    [SerializeField] private int ammoInMag;  // Current ammo in the magazine
    [SerializeField] private int reserveAmmo;  // Ammo remaining in the reserve
    [SerializeField] private float fireRate;  // Rate of fire for the gun

    [Header("Positional Data")]
    [SerializeField] private Vector3 originalPosition;  // Gun's original position
    [SerializeField] private Vector3 aimPosition;  // Position when aiming down sights (ADS)
    [SerializeField] private Quaternion originalRotation;  // Original rotation of the gun
    
    [Header("Recoil Stuff")]
    private Vector3 _currentRecoil;  // Current recoil being applied to the gun
    private Vector3 _targetRecoil;  // Target recoil, used for smoothing recoil over time

    [Header("Aiming")]
    private Coroutine _aimRoutine;  // Coroutine for the aiming transition
    private bool _isAiming;  // Flag to check if the player is aiming down sights

    [Header("FX")]
    [SerializeField] private ParticleSystem muzzleFlash;  // Muzzle flash effect
    [SerializeField] private GameObject bulletTrail;  // Bullet trail effect
    [SerializeField] private ParticleSystem bloodSplatter;  // Blood splatter effect for hits
    [SerializeField] private Animator anim;  // Animator for gun animations
    [SerializeField] private AudioSource audioSource;  // Audio source for gunshot sounds

    private UIManager _uiManager;  // Reference to the UI Manager
    [SerializeField] private AudioClip gunShot;  // Gunshot sound
    [SerializeField] private AudioClip _noAmmo;  // No ammo sound

    [Header("Camera Stuff")]
    [SerializeField] private float _DefaultFov;  // Default field of view (FOV) of the camera
    [SerializeField] private float adsFOV;  // Field of view when aiming down sights (ADS)
    private InputManager _input;  // Reference to the input manager for getting input

    void Start()
    {
        _input = InputManager.instance;  // Get the input manager instance

        // Register input actions for firing, aiming, and reloading
        _input.FireAction.performed += OnTriggerPulled;
        _input.FireAction.canceled += OnTriggerReleased;
        _input.AimAction.performed += OnAimPressed;
        _input.AimAction.canceled += OnAimReleased;
        _input.ReloadAction.performed += onReloadPressed;

        anim = GetComponent<Animator>();  // Get the animator component
        audioSource = GetComponent<AudioSource>();  // Get the audio source component
        
        originalPosition = transform.localPosition;  // Store the original position of the gun
        originalRotation = transform.localRotation;  // Store the original rotation of the gun

        aimPosition = data.aimPosition;  // Get the aiming position from the GunData

        _DefaultFov = mainCam.fieldOfView;  // Store the default FOV
        ammoInMag = data.magSize;  // Set the ammo in the mag from the GunData
        reserveAmmo = data.spareAmmo - data.magSize;  // Calculate reserve ammo after magazine is filled
        fireRate = data.fireRate;  // Set fire rate from the GunData
    }

    void Update()
    {
        ApplyRecoil();
        ResetRecoil();
        HandleSway(); // Apply the sway and recoil to the gun
    }

    #region Shooting

    // Trigger pressed, start shooting
    private float lastShotTime;
    private void OnTriggerPulled(InputAction.CallbackContext obj)
    {
        triggerDown = true;

        // Check if enough time has passed since the last shot to fire
        if (Time.time - lastShotTime >= 1f / fireRate)
        {
            if (ammoInMag > 0)
            {
                canFire = true;
                ammoInMag--;
                lastShotTime = Time.time; // Update the last shot time

                if (_isAiming)
                    ADSShoot();
                else
                    HipShoot();
            }
        }
    }


    // Trigger released, stop shooting
    private void OnTriggerReleased(InputAction.CallbackContext obj)
    {
        triggerDown = false;
        canFire = false;

        // Stop muzzle flash if it is playing
        if (muzzleFlash.isPlaying)
            muzzleFlash.Stop();
    }

    // ADS Shooting logic
    private void ADSShoot()
    {
        Vector3 direction = mainCam.transform.forward;  // Direction of the shot based on the camera's forward vector
        RaycastHit hit;

        // Raycast to detect hits
        if (Physics.Raycast(muzzle.position, direction, out hit))
        {
            if (hit.collider.CompareTag("Target"))
            {
                BasicEnemy targetScript = hit.collider.gameObject.GetComponent<BasicEnemy>();
                if (targetScript != null)
                    targetScript.TakeDamage(data.damage);  // Deal damage to the target

                // Blood splatter effect
                if (bloodSplatter != null)
                {
                    ParticleSystem blood = Instantiate(bloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
                    blood.Play();
                    Destroy(blood.gameObject, blood.main.duration);  // Destroy blood splatter after a set duration
                }
            }

            // Bullet trail effect
            GameObject trail = Instantiate(bulletTrail, muzzle.position, muzzle.rotation);
            StartCoroutine(MoveBulletTrail(trail, muzzle.position, hit.point));  // Move bullet trail towards the hit point
        }

        // Play muzzle flash and gunshot sound if ammo is available
        if (canFire && ammoInMag > 0)
        {
            muzzleFlash.Play();
            audioSource.PlayOneShot(gunShot);
        }
        else if (ammoInMag == 0)
        {
            audioSource.PlayOneShot(_noAmmo);  // Play no ammo sound
        }
    }

    // Hip shooting logic
    private void HipShoot()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);  // Raycast for shooting direction
        RaycastHit hit;

        // Raycast to detect hits
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Target"))
            {
                if (bloodSplatter != null)
                {
                    ParticleSystem blood = Instantiate(bloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
                    blood.Play();
                    Destroy(blood.gameObject, blood.main.duration);  // Destroy blood splatter after a set duration
                }

                BasicEnemy targetScript = hit.collider.gameObject.GetComponent<BasicEnemy>();
                if (targetScript != null)
                    targetScript.TakeDamage(data.damage);  // Deal damage to the target
            }

            // Bullet trail effect
            GameObject trail = Instantiate(bulletTrail, muzzle.position, muzzle.rotation);
            StartCoroutine(MoveBulletTrail(trail, muzzle.position, hit.point));  // Move bullet trail towards the hit point
        }

        // Play muzzle flash and gunshot sound if ammo is available
        if (canFire)
        {
            muzzleFlash.Play();
            audioSource.PlayOneShot(gunShot);
        }

        // Play no ammo sound when ammo runs out
        if (triggerDown && ammoInMag == 0)
        {
            audioSource.PlayOneShot(_noAmmo);
        }
    }

    // Move bullet trail to hit point
    private IEnumerator MoveBulletTrail(GameObject trail, Vector3 start, Vector3 end)
    {
        float time = 0f;
        float duration = 0.05f;  // Duration for the bullet trail to move

        // Move the trail towards the hit point
        while (time < 1f)
        {
            trail.transform.position = Vector3.Lerp(start, end, time);
            time += Time.deltaTime / duration;
            yield return null;
        }

        trail.transform.position = end;  // Set final position of the trail
        Destroy(trail, 0.5f);  // Destroy bullet trail after 0.5 seconds
    }

    #endregion

    #region Aiming

    // When the player aims, start the aiming process
    private void OnAimPressed(InputAction.CallbackContext obj)
    {
        _isAiming = true;
        if (_aimRoutine != null) StopCoroutine(_aimRoutine);
        _aimRoutine = StartCoroutine(AimDownSights());  // Start ADS transition
        mainCam.fieldOfView = adsFOV;  // Change FOV for ADS
    }

    // When the player releases aim, stop the aiming process
    private void OnAimReleased(InputAction.CallbackContext obj)
    {
        _isAiming = false;
        if (_aimRoutine != null) StopCoroutine(_aimRoutine);
        _aimRoutine = StartCoroutine(ReturnSightPosition());  // Return to original position
        mainCam.fieldOfView = _DefaultFov;  // Restore the default FOV
    }

    // Coroutine to handle aiming down sights (ADS)
    private IEnumerator AimDownSights()
    {
        float startFOV = mainCam.fieldOfView;
        float targetFOV = adsFOV;
        float time = 0f;
        float zoomTransitionSpeed = 1.5f;

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = aimPosition;

        while (time < 1f)
        {
            time += Time.deltaTime * zoomTransitionSpeed;
            float smoothTime = Mathf.SmoothStep(0f, 1f, time);

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, smoothTime);
            mainCam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, smoothTime);

            yield return null;
        }

        // Snap exactly to final position and FOV at the end
        transform.localPosition = endPosition;
        mainCam.fieldOfView = targetFOV;
    }


    // Coroutine to return the gun to its original position when not aiming
    private IEnumerator ReturnSightPosition()
    {
        float startFOV = mainCam.fieldOfView;
        float targetFOV = _DefaultFov;
        float time = 0f;
        float zoomTransitionSpeed = 1.5f;

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = originalPosition;

        while (time < 1f)
        {
            time += Time.deltaTime * zoomTransitionSpeed;
            float smoothTime = Mathf.SmoothStep(0f, 1f, time);

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, smoothTime);
            mainCam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, smoothTime);

            yield return null;
        }

        // Snap to final position and FOV
        transform.localPosition = endPosition;
        mainCam.fieldOfView = targetFOV;
    }



    #endregion

    #region Recoil

    // Apply recoil if the fire button is held down
    private void ApplyRecoil()
    {
        if (_input.FireAction.ReadValue<float>() > 0f)
        {
            // Randomly apply recoil on all axes
            _targetRecoil += new Vector3(Random.Range(-data.xRecoil, data.xRecoil), Random.Range(-data.yRecoil, data.yRecoil), Random.Range(-data.zRecoil, data.zRecoil));
        }
    }

    // Reset recoil over time (smoothing)
    private void ResetRecoil()
    {
        _targetRecoil = Vector3.Lerp(_targetRecoil, Vector3.zero, Time.deltaTime * data.recoilResetSpeed);
        _currentRecoil = Vector3.Lerp(_currentRecoil, _targetRecoil, Time.deltaTime * data.recoilResetSpeed);
        transform.localRotation = Quaternion.Euler(_currentRecoil) * originalRotation;  // Apply the recoil rotation to the gun
    }

    #endregion

    #region Sway

    // Apply camera sway and recoil effects when moving or shooting
    private void HandleSway()
    {
        // Don’t apply sway while aiming down sights
        

        float x = _input.Look.x;
        float y = _input.Look.y;

        var xAdj = Quaternion.AngleAxis(-data.swayIntensity * x, Vector3.up);
        var yAdj = Quaternion.AngleAxis(data.swayIntensity * y, Vector3.right);
        var zAdj = Quaternion.AngleAxis(-data.swayIntensity * x, Vector3.forward);

        var targetRotation = originalRotation * xAdj * yAdj * zAdj;

        transform.localRotation =
            Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * data.smoothing);
    }





    #endregion

    #region Reloading

    // Reload logic
    private void onReloadPressed(InputAction.CallbackContext obj)
    {
        if (reloading || ammoInMag == data.magSize || reserveAmmo <= 0)
            return;

        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        reloading = true;

        anim.SetTrigger("Reload");  // Play reload animation (if you have one)
       ;  // Play reload sound (if you have one)

        // Wait for reload duration
        yield return new WaitForSeconds(data.reloadTime);

        // Refill ammo
        int ammoNeeded = data.magSize - ammoInMag;
        if (reserveAmmo >= ammoNeeded)
        {
            ammoInMag += ammoNeeded;
            reserveAmmo -= ammoNeeded;
        }
        else
        {
            ammoInMag += reserveAmmo;
            reserveAmmo = 0;
        }

        reloading = false;
    }

    #endregion
}
