using UnityEngine;

public class BallisticProjectileCannon : ProjectileCannon {
	public PhysicsProjectile projectile;
	public Transform muzzle;
	public Transform muzzleEnd;
	public float projectileSpeed = 2f;
	public float rotationSpeed = 10f;

	private Transform m_target;
	private Vector3 m_tVel;

	private Vector3 m_pVel;

	private void Awake() {
		ProjectilePool.AddPool(projectile);
	}

	private void FixedUpdate() {
		if (m_target == null) {
			return;
		}
		if (!ArtilleryUtils.TryCalcVelBallistic(m_target.position, m_tVel, muzzleEnd.position, projectileSpeed, out m_pVel)) {
			return;
		}
		muzzle.rotation = Quaternion.RotateTowards(
			muzzle.rotation,
			Quaternion.LookRotation(m_pVel),
			rotationSpeed * Time.fixedDeltaTime);
	}

	public override void SetTarget(Transform target) {
		m_target = target;
		m_tVel = ArtilleryUtils.CalcTargetVel(m_target);
	}

	public override void Shoot() {
		var p = ProjectilePool.Get<PhysicsProjectile>();
		p.body.useGravity = true;
		p.transform.SetPositionAndRotation(muzzleEnd.position, muzzleEnd.rotation);
		p.body.linearVelocity = m_pVel;
		p.Destroyed += () => ProjectilePool.Release(p);
	}
}