using UnityEngine;

public class ActivateParticleSystem : MonoBehaviour
{
	public ParticleSystem particleSystem; // Reference to the Particle System
	public KeyCode inputKey = KeyCode.E; // The input key to activate the particle system

	void Update()
	{
		// Check if the input key is pressed
		if (Input.GetKeyDown(inputKey))
		{
			ActivateParticles();
		}
	}

	// Function to play the particle system
	private void ActivateParticles()
	{
		if (particleSystem != null)
		{
			particleSystem.Play(); // Play the particle system
		}
	}
}