using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.UI;
using TMPro;

public class GunController : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private GunData data;

    [Header("Shot Origins")] 
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform sight;

    [Header("Operational State")] 
    [SerializeField] private bool canFire;
    [SerializeField] private bool triggerDown;
    [SerializeField] private bool reloading;

    [Header("Ammo Info")] 
    [SerializeField] private int ammoInMag;
    [SerializeField] private int reserveAmmo;
    [SerializeField] private float fireRate;
    
   


    [Header("Positional Data")] 
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private Quaternion originalRotation;
    
    [Header("Recoil Stuff")]
    private Vector3 _currentRecoil;
    private Vector3 _targetRecoil;


    [Header("Aiming")]
    private Coroutine _aimRoutine;
    private bool _isAiming;

    [Header("FX")] 
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject bulletTrail;
    [SerializeField] private ParticleSystem bloodSplatter; 
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audioSource;

    private UIManager _uiManager;
   [SerializeField] private AudioClip _gunShot; // *gun fires*
   [SerializeField] private AudioClip _noAmmo; // *click click no ammo in gun sound effect*
    

    [Header("Camera Stuff")]
    [ SerializeField] private float _DefaultFov;

    [SerializeField] private float adsFOV;
    private InputManager _input;
    
    // Start is called before the first frame update
    void Start()
    {
        _input = InputManager.instance;

        _input.FireAction.performed += OnTriggerPulled;
        _input.FireAction.canceled += OnTriggerReleased;
        _input.AimAction.performed += OnAimPressed;
        _input.AimAction.canceled += OnAimReleased;
        _input.ReloadAction.performed += OnReloadPressed;
        // initialize the audio source
        // initialize the animator
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        // cache the original position and rotation to reset to after recoiling/adsing
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        aimPosition = data.aimPosition;

        _DefaultFov = mainCam.fieldOfView;
        mainCam.fieldOfView = _DefaultFov;
        ammoInMag = data.magSize;
        reserveAmmo = data.spareAmmo - data.magSize;
        fireRate = data.fireRate;
        
        float secondsBetweenShots = 60f / data.fireRate; // this is to make sure that its treating fire rate as RPM instead of RPH
        


    }

    // Update is called once per frame
    void Update()
    {
        // what should happen all the time?
        
        ResetRecoil();
        HandleSway();
        

    }
    
    #region Shooting

    private void OnTriggerPulled(InputAction.CallbackContext obj)
    {
        // can I shoot?
        if (ammoInMag >0)
        {
            canFire = true;
            ammoInMag--;
            if (_isAiming)
            {
                ADSShoot();
            }
            else
            {
                HipShoot();
            }
            
        }
        
        

        

        // if I can, which mode should I shoot in?
    }

    private void OnTriggerReleased(InputAction.CallbackContext obj)
    {
        // stop shooting
        if (ammoInMag<=0)
        {
            canFire = false;
            
            
        }

        if (muzzleFlash.isPlaying)
        {
            muzzleFlash.Stop();
        }

        
    }
    private void ADSShoot()
    {
        // Perform hit detection using a raycast
        Vector3 direction = mainCam.transform.forward;
        RaycastHit hit;
    
        // Raycast to detect collision
        if (Physics.Raycast(muzzle.position, direction, out hit))
        {
            Debug.Log("Ray hit: " + hit.collider.name);

            // Check if the hit object has the "Target" tag
            if (hit.collider.CompareTag("Target"))
            {
                // Get the Target component of the hit object
                BasicEnemy targetScript = hit.collider.gameObject.GetComponent<BasicEnemy>();

                // Ensure the Target component is found before calling TakeDamage
                if (targetScript != null)
                {
                    // Apply damage to the target
                    targetScript.TakeDamage(data.damage);
                    Debug.Log("Target hit! Applying damage.");
                }
                else
                {
                    Debug.LogError("Target script not found on " + hit.collider.name);
                }

                // Instantiate blood splatter at the hit point
                if (bloodSplatter != null)
                {
                    ParticleSystem blood = Instantiate(bloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
                    blood.Play(); // Play the particle system
                    Destroy(blood.gameObject, blood.main.duration); // Destroy it after it finishes
                }
            }

            // Instantiate the bullet trail
            GameObject trail = Instantiate(bulletTrail, muzzle.position, muzzle.rotation);
            StartCoroutine(MoveBulletTrail(trail, muzzle.position, hit.point));
        }

        // Apply recoil and play gunshot sound if ammo is available
        if (canFire && ammoInMag > 0)
        {
            muzzleFlash.Play();
            audioSource.PlayOneShot(_gunShot);
            
        }
        // Play "no ammo" sound only if ammo is empty
        else if (ammoInMag == 0)
        {
            audioSource.PlayOneShot(_noAmmo);
        }
    }




    private void HipShoot()
    {
        // Perform hit detection using a raycast
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;
    
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.transform.name);

            // Check if the hit object is a target (e.g., an enemy)
            if (hit.collider.CompareTag("Target"))
            {
                // Instantiate the blood splatter at the hit point
                if (bloodSplatter != null)
                {
                    // Instantiate the blood splatter at the hit point with correct rotation
                    ParticleSystem blood = Instantiate(bloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
                    blood.Play();  // Start playing the particle system
                    Destroy(blood.gameObject, blood.main.duration);  // Destroy the blood splatter after it finishes
                }

                // Apply damage if it's an enemy
                BasicEnemy targetScript = hit.collider.gameObject.GetComponent<BasicEnemy>();
                if (targetScript != null)
                {
                    targetScript.TakeDamage(data.damage);
                }
            }

            // Instantiate the bullet trail
            GameObject trail = Instantiate(bulletTrail, muzzle.position, muzzle.rotation);
            StartCoroutine(MoveBulletTrail(trail, muzzle.position, hit.point));
        }

        // Apply recoil
        if (canFire)
        {
            muzzleFlash.Play();
            audioSource.PlayOneShot(_gunShot);
            
        }
        else
        {
            audioSource.PlayOneShot(_noAmmo);
        }
    }


    private void ApplyRecoil()
    {
        if (_input.FireAction.ReadValue<float>() > 0f)
        {
            
        }
    }







   

    private void ResetRecoil()
    {
        // Smoothly reduce target recoil to zero (like the gun returning to resting position)
        _targetRecoil = Vector3.Lerp(_targetRecoil, Vector3.zero, Time.deltaTime * data.recoilResetSpeed);

        // Smoothly move current recoil toward target recoil
        _currentRecoil = Vector3.Lerp(_currentRecoil, _targetRecoil, Time.deltaTime * data.recoilResetSpeed);

        // Apply the recoil to the gun's rotation
        transform.localRotation = Quaternion.Euler(_currentRecoil) * originalRotation;
    }


        private void OnReloadPressed(InputAction.CallbackContext obj)
        {
            if (!reloading && reserveAmmo >0 && ammoInMag <data.magSize )
            {
                StartCoroutine(Reload());
            }

            if (reloading)
            {
                
            }

            
        }

        private IEnumerator Reload()
        {
            reloading = true;

            yield return new WaitForSeconds(data.reloadTime);

            int ammoNeeded = data.magSize - ammoInMag;
            int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

            ammoInMag += ammoToReload;
            reserveAmmo -= ammoToReload;
        }
        private IEnumerator MoveBulletTrail(GameObject trail, Vector3 start, Vector3 end)
        {
            float time = 0f;
            float duration = 0.05f; // Tune this for bullet speed

            while (time < 1f)
            {
                trail.transform.position = Vector3.Lerp(start, end, time);
                time += Time.deltaTime / duration;
                yield return null;
            }

            trail.transform.position = end;
            Destroy(trail, 0.5f);
        }
        

        #endregion

        #region Aiming

        private void OnAimPressed(InputAction.CallbackContext obj)
        {
            // interpolate between the original position of the gun and the aim position
            // using the aim coroutine.
            _isAiming = true;
            if (_aimRoutine != null) StopCoroutine(_aimRoutine);
            _aimRoutine = StartCoroutine(AimDownSights());
            mainCam.fieldOfView = 20;
        }

        private void OnAimReleased(InputAction.CallbackContext obj)
        {
            // opposite of aim pressed.
            _isAiming = false;
            if (_aimRoutine != null) StopCoroutine(_aimRoutine);
            _aimRoutine = StartCoroutine(ReturnSightPosition());
            mainCam.fieldOfView = 60;
        }

        private IEnumerator AimDownSights()
        {
            // Save the default FOV before aiming down sights
            float startFOV = mainCam.fieldOfView; // This stores the current FOV
            float targetFOV = adsFOV; // The target FOV when aiming down sights

            float zoomTransitionSpeed = 1.5f; // Speed for zoom transition
            float time = 0f; // A variable to track the transition time

            while (_isAiming) // While the player is aiming
            {
                time += Time.deltaTime *
                        zoomTransitionSpeed; // Increment the time with deltaTime and zoom transition speed

                // Smooth transition factor using SmoothStep (this will ease the transition)
                float smoothTime = Mathf.SmoothStep(0f, 1f, time);
                transform.localPosition =
                    Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * data.adsSpeed);


                // Smoothly interpolate the camera's FOV to the ADS FOV
                mainCam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, smoothTime);

                // Exit the loop once the transition completes (smoothTime reaches 1)
                if (smoothTime >= 1f)
                    break;

                yield return null; // Wait for the next frame
            }
        }


        private IEnumerator ReturnSightPosition()
        {
            // Initial variables
            float startFOV = mainCam.fieldOfView;
            float targetFOV = _DefaultFov; // The FOV to return to (when not aiming)
            float zoomTransitionSpeed = 1.5f;
            float time = 0f;
            float adsSpeed = data.adsSpeed;

            // Smooth transition for both FOV and position
            while (!_isAiming)
            {
                time += Time.deltaTime * zoomTransitionSpeed; // Increment time with transition speed

                // Calculate smooth transition factor (0 to 1)
                float smoothTime = Mathf.SmoothStep(0f, 1f, time);

                // Smoothly interpolate the gun's position back to the original position (keeping the original transition speed)
                transform.localPosition =
                    Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);

                // Smoothly interpolate the camera's FOV back to the default FOV using smoothTime
                mainCam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, smoothTime);

                // Exit loop once transition is complete
                if (smoothTime >= 1f)
                    break;

                yield return null;
            }
        }


        #endregion

        #region Sway

        private void HandleSway()
        {
            float x = _input.Look.x;
            float y = _input.Look.y;

            // compute adjustment rotations based on sway intensity
            // and input amount. 
            var xAdj = Quaternion.AngleAxis(-data.swayIntensity * x, Vector3.up);
            var yAdj = Quaternion.AngleAxis(data.swayIntensity * y, Vector3.right);
            var zAdj = Quaternion.AngleAxis(-data.swayIntensity * x, Vector3.forward);

            // then create a true target rotation
            var targetRotation = originalRotation * xAdj * yAdj * zAdj;

            // then apply it using a Lerp.
            transform.localRotation =
                Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * data.smoothing);
        }

        #endregion

        #region Damage

        

        #endregion
        
        
    }