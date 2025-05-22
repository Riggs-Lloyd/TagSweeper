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

    private Tile tile;

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
            if (hit.collider.gameObject.transform.parent.CompareTag("Taggable"))
            {
                Debug.Log(hit.collider.gameObject.name + " was hit");

                wallPosition = hit.collider.gameObject.transform.position;
                wallRotation = hit.collider.gameObject.transform.rotation;
                newSprayPosition = hit.point; // Set the spray position to the hit point
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Transform hitTransform = hit.collider.transform;
                    Tile t = hit.collider.GetComponentInParent<Tile>();
                    Collider col = hit.collider;
                    
                    Vector3 center = col.bounds.center;
                    Vector3 extent = col.bounds.extents;
                    float pushOut = 0.01f; // Slight offset to place just outside the surface

                    // Define the four directions and matching extents
                    Vector3[] directions = new Vector3[]
                    {
                        hitTransform.forward,
                        -hitTransform.forward,
                        hitTransform.right,
                        -hitTransform.right
                    };

                    Vector3[] offsets = new Vector3[]
                    {
                        new Vector3(0, 0, extent.z + pushOut),
                        new Vector3(0, 0, -extent.z - pushOut),
                        new Vector3(extent.x + pushOut, 0, 0),
                        new Vector3(-extent.x - pushOut, 0, 0)
                    };

                    for (int i = 0; i < directions.Length; i++)
                    {
                        Vector3 decalPos = center + hitTransform.rotation * offsets[i];
                        Quaternion decalRot = Quaternion.LookRotation(-directions[i]);

                        CreateNewRandomDecal(decalPos, decalRot, t);
                    }

                    hitTransform.parent.tag = "Tagged";
                    // Before creating a new decal, ensure we don't exceed maxSprayCount
                    //if (activeDecals.Count >= maxSprayCount)
                   //{
                        // Remove the oldest decal (first in the list) if we've exceeded the limit
                        //Destroy(activeDecals[0].gameObject); // Destroy the oldest decal
                        //activeDecals.RemoveAt(0); // Remove it from the list
                   // }

                    // Instantiate a new random decal at the spray position
                    hit.collider.gameObject.transform.parent.tag = "Tagged";
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
    void CreateNewRandomDecal(Vector3 position, Quaternion rotation, Tile t)
    { 
        //Get hit.collider.GameObject. check (Int Value) If int value = to certain number select spray of detection range, random cosmetic spray if bomb not within 4. If Int value = to bomb value explode.
        // Check if there are decal prefabs available
        if (_decalProjectors.Length > 0)
        {

            if(t != null)
            { Debug.Log("t is not null");
                    DecalProjector newDecal;
                    switch (t.counter)
                    {
                        case 0:
                            newDecal = Instantiate(_decalProjectors[0], position, rotation);
                            activeDecals.Add(newDecal);
                            Debug.Log("Trying to draw " + t.counter + " decal");
                            break;
                        case 1:
                            newDecal = Instantiate(_decalProjectors[1], position, rotation);
                            activeDecals.Add(newDecal);
                            Debug.Log("Trying to draw " + t.counter + " decal");

                            break;
                        case 2:
                            newDecal = Instantiate(_decalProjectors[2], position, rotation);
                            activeDecals.Add(newDecal);
                            Debug.Log("Trying to draw " + t.counter + " decal");
                            break;
                        case 3:
                            newDecal = Instantiate(_decalProjectors[3], position, rotation);
                            activeDecals.Add(newDecal);
                            Debug.Log("Trying to draw " + t.counter + " decal");

                            break;
                        case 4:
                            newDecal = Instantiate(_decalProjectors[4], position, rotation);
                            activeDecals.Add(newDecal);
                            break;
                        case 5:
                            newDecal = Instantiate(_decalProjectors[5], position, rotation);
                            activeDecals.Add(newDecal);

                            break;
                        case 6:
                            newDecal = Instantiate(_decalProjectors[6], position, rotation);
                            activeDecals.Add(newDecal);
                            break;
                        case 7:
                            newDecal = Instantiate(_decalProjectors[7], position, rotation);
                            activeDecals.Add(newDecal);

                            break;
                        case 8:
                            newDecal = Instantiate(_decalProjectors[8], position, rotation);
                            activeDecals.Add(newDecal);
                            break;
                    }
            } else
            {
                Debug.Log("T was null");
            }
            
            //Access every tile and check if bomb is true for any of these tiles
            //Locate nearest tile that bomb is true and find its distance
            //Take this distance and access decal equal to this distance
            //Create new decal on object
        }
        else
        {
            //Debug.LogError("No decal projectors are assigned in the array!");
        }
    }
}
