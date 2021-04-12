using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class HealthScript : MonoBehaviour
{
	public int hp = 1;
	
	public bool isEnemy = true;
	
	public void Damage(int damageCount)
	{
		hp -= damageCount;

		if (hp <= 0)
		{
			SpecialEffectsHelper.Instance.Explosion(transform.position);
			SoundEffectsHelper.Instance.MakeExplosionSound();
			// Dead!
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			// Avoid friendly fire
			if (shot.isEnemyShot != isEnemy)
			{
				Damage(shot.damage);

				// Destroy the shot
				Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
			}
		}
	}
}