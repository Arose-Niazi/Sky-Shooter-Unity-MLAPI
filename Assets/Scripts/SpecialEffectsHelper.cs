using UnityEngine;


public class SpecialEffectsHelper : MonoBehaviour
{
	public static SpecialEffectsHelper Instance;

	public ParticleSystem smokeEffect;
	public ParticleSystem fireEffect;

	void Awake()
	{
		// Register the singleton
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of SpecialEffectsHelper!");
		}

		Instance = this;
	}
	
	public void Explosion(Vector3 position)
	{
		// Smoke on the water
		instantiate(smokeEffect, position);

		// Tu tu tu, tu tu tudu

		// Fire in the sky
		instantiate(fireEffect, position);
	}
	
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
	{
		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position,
			Quaternion.identity
		);

		// Make sure it will be destroyed
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.main.startLifetime.constant
		);

		return newParticleSystem;
	}
}