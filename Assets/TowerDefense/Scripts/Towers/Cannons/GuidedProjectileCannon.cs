using UnityEngine;

public class GuidedProjectileCannon : ProjectileCannon {
	[SerializeField] private Transform m_muzzle;
	[SerializeField] private GuidedProjectile m_projectile;

	private Transform m_target;

	private void Awake() {
		ProjectilePool.AddPool(m_projectile);
	}

	public override void SetTarget(Transform target) {
		m_target = target;
	}

	public override void Shoot() {
		var p = ProjectilePool.Get<GuidedProjectile>();
		p.transform.SetPositionAndRotation(m_muzzle.position, m_muzzle.rotation);
		p.target = m_target;
		p.Destroyed += () => ProjectilePool.Release(p);
	}
}