using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class HealthScript : NetworkBehaviour
{
	public int hp = 1;
	
	public bool isEnemy = true;
	
	public void Damage(int damageCount)
	{
		hp -= damageCount;

		if (hp <= 0)
		{
			// Dead!
			if (IsClient)
			{
				EffectServerRpc(transform.position);
				DeleteServerRpc();
			}
			else
			{
				SpecialEffectsHelper.Instance.Explosion(transform.position);
				SoundEffectsHelper.Instance.MakeExplosionSound();
				Destroy(gameObject);
			}
				
			
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
				if (IsServer)
					shot.GetComponent<NetworkObject>().Despawn(true);
				else
					Destroy(shot.gameObject);
			}
		}
	}

	[ServerRpc(RequireOwnership = false)]
	private void EffectServerRpc(Vector3 pos)
	{
		EffectClientRpc(pos);
	}

	[ClientRpc]
	private void EffectClientRpc(Vector3 pos)
	{
		SpecialEffectsHelper.Instance.Explosion(pos);
		SoundEffectsHelper.Instance.MakeExplosionSound();
	}
	
	[ServerRpc(RequireOwnership = false)]
	private void DeleteServerRpc()
	{
		gameObject.GetComponent<NetworkObject>().Despawn(true);
	}
}

