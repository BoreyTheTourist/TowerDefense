using UnityEngine;

public class StraightProjectileCannon : ProjectileCannon {
	public PhysicsProjectile projectile;
	public Transform muzzle;
	public Transform muzzleEnd;
	public float projectileSpeed = 2f;
	public float rotationSpeed = 10f;

	private Transform m_target;
	private Vector3 m_tVel;

	private void Awake() {
		ProjectilePool.AddPool(projectile);
	}

	private void FixedUpdate() {
		if (m_target == null) {
			return;
		}
		var tDir = m_target.position - muzzleEnd.position;
		if (!ArtilleryUtils.TryCalcVelStraight(tDir, m_tVel, projectileSpeed, out var hd)) {
			return;
		}
		muzzle.rotation = Quaternion.RotateTowards(
			muzzle.rotation,
			Quaternion.LookRotation(hd),
			rotationSpeed * Time.fixedDeltaTime);
	}

	public override void SetTarget(Transform target) {
		m_target = target;
		m_tVel = ArtilleryUtils.CalcTargetVel(m_target);
	}

	public override void Shoot() {
		var p = ProjectilePool.Get<PhysicsProjectile>();
		p.body.useGravity = false;
		p.transform.SetPositionAndRotation(muzzleEnd.position, muzzleEnd.rotation);
		p.body.linearVelocity = muzzleEnd.forward * projectileSpeed;
		p.Destroyed += () => ProjectilePool.Release(p);
	}
}