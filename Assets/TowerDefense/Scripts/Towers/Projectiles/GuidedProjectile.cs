using UnityEngine;

public class GuidedProjectile : Projectile {
	public Transform target;
	public float speed = 0.2f;

	private void FixedUpdate() {
		if (target == null) {
			OnDestroyed();
			return;
		}

		var translation = target.transform.position - transform.position;
		if (translation.magnitude > speed) {
			translation = translation.normalized * speed;
		}
		transform.Translate (translation);
	}
}