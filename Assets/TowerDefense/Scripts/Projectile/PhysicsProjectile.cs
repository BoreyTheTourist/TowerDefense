using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsProjectile : Projectile {
	private Rigidbody m_body;
	public Rigidbody body {
		get {
			if (m_body == null) {
				m_body = GetComponent<Rigidbody>();
			}
			return m_body;
		}
	}
}