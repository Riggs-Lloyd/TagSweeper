using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")] 
    [SerializeField] private TMP_Text ammoText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateAmmoUI(int ammoInMag, int reserveAmmo)
    {
        ammoText.text = $"{ammoInMag}/{reserveAmmo}";
    }
}
