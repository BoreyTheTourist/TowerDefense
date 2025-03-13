using UnityEngine;

public class BallisticProjectileCannon : ProjectileCannon {
	[Header("Projectile")]
	public PhysicsProjectile projectile;
	public float latSpeed = 7f;
	public bool useGravity = false;

	[Header("Cannon")]
	public Transform muzzle;
	public Transform muzzleEnd;
	public float rotationSpeed = 30f;

	private Transform m_target;
	private Driver m_driver;
	private Vector3 m_tVel;
	private Vector3 m_pVel;

	private System.Func<bool> TryCalcVel;
	private System.Func<Vector3> ProjVel;

	private void Awake() {
		ProjectilePool.AddPool(projectile);

		if (useGravity) {
			// very not effective and cool fire straight from muzzle -
			// accuracy tolerancy very low and visually not so much perfect
			ProjVel = () => m_pVel;
			TryCalcVel = () => {
				return ArtilleryUtils.TryCalcVelBallistic(
					m_target.position,
					m_tVel,
					muzzleEnd.position,
					latSpeed,
					out m_pVel);
			};
		} else {
			ProjVel = () => muzzleEnd.forward * latSpeed;
			TryCalcVel = () => {
				var tDir = m_target.position - muzzleEnd.position;
				return ArtilleryUtils.TryCalcVelStraight(tDir, m_tVel, latSpeed, out m_pVel);
			};
		}
	}

	private void FixedUpdate() {
		if (m_target == null) {
			return;
		}
		if (!TryCalcVel()) {
			return;
		}
		muzzle.rotation = Quaternion.RotateTowards(
			muzzle.rotation,
			Quaternion.LookRotation(m_pVel),
			rotationSpeed * Time.fixedDeltaTime);
	}

	private void ChangeVelocity(Vector3 vel) {
		m_tVel = vel;
	}

	public override void SetTarget(Transform target) {
		m_target = target;
		if (m_driver) {
			m_driver.VelocityChanged -= ChangeVelocity;
		}
		m_tVel = CalcTargetVel(m_target, out m_driver);
		if (m_driver) {
			m_driver.VelocityChanged += ChangeVelocity;
		}
	}

	public override void Shoot() {
		var p = ProjectilePool.Get<PhysicsProjectile>();
		p.body.useGravity = useGravity;
		p.transform.SetPositionAndRotation(muzzleEnd.position, muzzleEnd.rotation);
		p.body.linearVelocity = ProjVel();
		p.Destroyed += () => ProjectilePool.Release(p);
	}
	
	public Vector3 CalcTargetVel(Transform target, out Driver driver) {
		driver = target.GetComponentInParent<Driver>();
		if (driver != null) {
			return driver.velocity;
		}
		var posb = target.position;
		
		var mode = Physics.simulationMode;
		Physics.simulationMode = SimulationMode.Script;
		Physics.Simulate(Time.fixedDeltaTime);
		Physics.simulationMode = mode;

		return (target.position - posb) / Time.fixedDeltaTime;
	}
}