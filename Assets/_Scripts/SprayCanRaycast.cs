using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SprayCanRaycast : MonoBehaviour
{
    private Ray _ray;
    private float maxDistance = 10;
    public LayerMask _layerMask;
    
    // Array to hold different decal prefabs
    public DecalProjector[] _decalProjectors; 

    public GameObject _RayCaster;
    private Vector3 newSprayPosition;
    private Vector3 wallPosition;
    private Quaternion wallRotation;
    private bool wallSprayed;

    // List to store all active decals
    private List<DecalProjector> activeDecals = new List<DecalProjector>();

    // Maximum number of decals that can be sprayed at a time
    public int maxSprayCount = 5;

    void Start()
    {
        _ray = new Ray(transform.position, transform.forward);
    }

    void Update()
    {
        // Update the ray's position and direction every frame
        _ray.origin = transform.position;
        _ray.direction = transform.forward;

        CheckForColliders();
    }

    void CheckForColliders()
    {
        if (Physics.Raycast(_ray, out UnityEngine.RaycastHit hit, maxDistance, _layerMask))
        {
            if (hit.collider.gameObject.CompareTag("Taggable"))
            {
                Debug.Log(hit.collider.gameObject.name + " was hit");

                wallPosition = hit.collider.gameObject.transform.position;
                wallRotation = hit.collider.gameObject.transform.rotation;
                newSprayPosition = hit.point; // Set the spray position to the hit point
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Before creating a new decal, ensure we don't exceed maxSprayCount
                    //if (activeDecals.Count >= maxSprayCount)
                   //{
                        // Remove the oldest decal (first in the list) if we've exceeded the limit
                        //Destroy(activeDecals[0].gameObject); // Destroy the oldest decal
                        //activeDecals.RemoveAt(0); // Remove it from the list
                   // }

                    // Instantiate a new random decal at the spray position
                    CreateNewRandomDecal(wallPosition, wallRotation);
                    hit.collider.gameObject.tag = "Tagged";
                }
                
            }
        }
        else
        {
            // No hit found within the max distance
            //Debug.Log("No collider hit within max distance.");
        }
    }

    // Create a new random decal at the specified position
    void CreateNewRandomDecal(Vector3 position, Quaternion rotation)
    { 
        //Get hit.collider.GameObject. check (Int Value) If int value = to certain number select spray of detection range, random cosmetic spray if bomb not within 4. If Int value = to bomb value explode.
        // Check if there are decal prefabs available
        if (_decalProjectors.Length > 0)
        {
            // Randomly select a decal prefab from the array
            int randomIndex = Random.Range(0, _decalProjectors.Length);

            // Instantiate the selected decal prefab and set its position
            DecalProjector newDecal = Instantiate(_decalProjectors[randomIndex], position, rotation);
            activeDecals.Add(newDecal); // Add the new decal to the list of active decals
        }
        else
        {
            //Debug.LogError("No decal projectors are assigned in the array!");
        }
    }
}
