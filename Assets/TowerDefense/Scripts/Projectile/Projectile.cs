using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour {
	[SerializeField] protected float m_damage;
	public event System.Action Destroyed;

	private void Start() {
		var col = GetComponent<Collider>();
		col.isTrigger = true;
	}

	private void OnTriggerEnter(Collider other) {
		var dmg = other.GetComponentInParent<Damageable>();
		if (dmg != null) {
			dmg.TakeDamage(m_damage);
		}
		Destroyed?.Invoke();
	}

	protected void OnDestroyed() {
		Destroyed?.Invoke();
	}
}