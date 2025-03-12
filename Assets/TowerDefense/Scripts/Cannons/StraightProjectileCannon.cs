using UnityEngine;

public class StraightProjectileCannon : ProjectileCannon {
	public PhysicsProjectile projectile;
	public Transform muzzle;
	public Transform muzzleEnd;
	public float projectileSpeed = 2f;
	public float rotationSpeed = 10f;

	private Transform m_target;
	private Vector3 m_targetVel;

	private void Awake() {
		ProjectilePool.AddPool(projectile);
	}

	private void FixedUpdate() {
		if (m_target == null) {
			return;
		}
		if (!TryCalcHitDir(m_target.position - muzzleEnd.position, m_targetVel, projectileSpeed, out var hd)) {
			return;
		}
		muzzle.rotation = Quaternion.RotateTowards(muzzle.rotation, Quaternion.LookRotation(hd), rotationSpeed * Time.fixedDeltaTime);
	}

	public override void SetTarget(Transform target) {
		m_target = target;
		CalcTargetVel();
	}

	public override void Shoot() {
		var p = ProjectilePool.Get<PhysicsProjectile>();
		p.body.useGravity = false;
		p.transform.SetPositionAndRotation(muzzleEnd.position, muzzleEnd.rotation);
		p.body.linearVelocity = muzzleEnd.forward * projectileSpeed;
		p.Destroyed += () => ProjectilePool.Release(p);
	}

	// pPos + pVel * t = tPos + tVel * t
	// |pVel| = pSpeed = Sqrt(Dot(pVel, pVel))
	// tDir = tPos - pPos
	// 
	// Dot(pVel, pVel) * t^2 = Dot(tDir, tDir) + 2t * Dot(tDir, tVel) + Dot(tVel, tVel) * t^2
	// t^2*(Dot(tVel, tVel) - pSpeed^2) + t*2Dot(tDir, tVel) + Dot(tDir, tDir) = 0
	private bool TryCalcHitDir(Vector3 tDir, Vector3 tVel, float pSpeed, out Vector3 hd) {
		hd = Vector3.zero;
		float a = Vector3.Dot(tVel, tVel) - pSpeed * pSpeed;
		float b = 2 * Vector3.Dot(tDir, tVel);
		float c = Vector3.Dot(tDir, tDir);
		if (!TrySolveQuadratic(a, b, c, out var res)) {
			return false;
		}

		float t = res.x;
		if (res.x < 0 && res.y < 0) {
			return false;
		} else if (res.x > 0 && res.y > 0) {
			t = Mathf.Min(res.x, res.y);
		} else if (res.x < 0) {
			t = res.y;
		}

		hd = tDir / t + tVel;
		return true;
	}

	private void CalcTargetVel() {
		if (m_target.TryGetComponent<Monster>(out var m)) {
			m_targetVel = m.velocity;
			return;
		}
		var posb = m_target.position;
		
		var mode = Physics.simulationMode;
		Physics.simulationMode = SimulationMode.Script;
		Physics.Simulate(Time.fixedDeltaTime);
		Physics.simulationMode = mode;

		m_targetVel = (m_target.position - posb) / Time.fixedDeltaTime;
	}

	private bool TrySolveQuadratic(float a, float b, float c, out Vector2 res) {
		res = Vector2.zero;
		float d = b*b - 4*a*c;
		switch (Mathf.Sign(d)) {
			case -1:
			return false;

			case 0:
			res.x = res.y = -b / (2*a);
			return true;

			case 1:
			d = Mathf.Sqrt(d);
			var r = 1 / (2*a);
			res.x = (-b - d) * r;
			res.y = (-b + d) * r;
			return true;
		}
		return false;
	}
}