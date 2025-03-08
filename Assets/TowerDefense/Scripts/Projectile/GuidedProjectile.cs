using UnityEngine;

public class GuidedProjectile : Projectile {
	public Transform m_target;
	public float m_speed = 0.2f;

	void Update () {
		if (!m_target.gameObject.activeSelf) {
			OnDestroyed();
			return;
		}

		var translation = m_target.transform.position - transform.position;
		if (translation.magnitude > m_speed) {
			translation = translation.normalized * m_speed;
		}
		transform.Translate (translation);
	}
}