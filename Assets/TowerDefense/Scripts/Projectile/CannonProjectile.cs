using UnityEngine;

public class CannonProjectile : Projectile {
	public float m_speed = 0.2f;

	void Update () {
		var translation = transform.forward * m_speed;
		transform.Translate (translation);
	}
}