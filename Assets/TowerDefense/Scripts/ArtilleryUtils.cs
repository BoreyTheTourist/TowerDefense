using UnityEngine;

public static class ArtilleryUtils {
	public static bool TryCalcVelBallistic(Vector3 tPos, Vector3 tVel, Vector3 pPos, float latSpeed, out Vector3 vel) {
		vel = Vector3.zero;

		var tDirxz = tPos - pPos;
		var tVelxz = tVel;
		tDirxz.y = tVelxz.y = 0f;
		if (!TryCalcInterTime(tDirxz, tVelxz, latSpeed, out var t)) {
			return false;
		}

		vel = tDirxz / t + tVelxz;

		// same motion equation, but for y only:
		// yend = y0 + vy*t + .5f*g*t^2
		float yend = tPos.y + tVel.y * t;
		float g = Physics.gravity.y;
		vel.y = (yend - pPos.y) / t - .5f*g*t;
		return true;
	}

	public static bool TryCalcVelStraight(Vector3 tDir, Vector3 tVel, float pSpeed, out Vector3 vel) {
		vel = Vector3.zero;
		if (!TryCalcInterTime(tDir, tVel, pSpeed, out var t)) {
			return false;
		}

		vel = tDir / t + tVel;
		return true;
	}

	public static Vector3 CalcTargetVel(Transform target) {
		var d = target.GetComponentInParent<Driver>();
		if (d != null) {
			return d.velocity;
		}
		var posb = target.position;
		
		var mode = Physics.simulationMode;
		Physics.simulationMode = SimulationMode.Script;
		Physics.Simulate(Time.fixedDeltaTime);
		Physics.simulationMode = mode;

		return (target.position - posb) / Time.fixedDeltaTime;
	}

	// pPos + pVel * t = tPos + tVel * t
	// |pVel| = pSpeed = Sqrt(Dot(pVel, pVel))
	// tDir = tPos - pPos
	// 
	// Dot(pVel, pVel) * t^2 = Dot(tDir, tDir) + 2t * Dot(tDir, tVel) + Dot(tVel, tVel) * t^2
	// t^2*(Dot(tVel, tVel) - pSpeed^2) + t*2Dot(tDir, tVel) + Dot(tDir, tDir) = 0
	/// <summary>
	/// </summary>
	/// <param name="tDir"></param>
	/// <param name="tVel"></param>
	/// <param name="pSpeed"></param>
	/// <param name="t"></param>
	/// <returns>Min time of intersection if possible</returns>
	private static bool TryCalcInterTime(Vector3 tDir, Vector3 tVel, float pSpeed, out float t) {
		t = 0;
		float a = Vector3.Dot(tVel, tVel) - pSpeed * pSpeed;
		float b = 2 * Vector3.Dot(tDir, tVel);
		float c = Vector3.Dot(tDir, tDir);
		if (!TrySolveQuadratic(a, b, c, out var res)) {
			return false;
		}

		t = res.x;
		if (res.x < 0 && res.y < 0) {
			return false;
		} else if (res.x > 0 && res.y > 0) {
			t = Mathf.Min(res.x, res.y);
		} else if (res.x < 0) {
			t = res.y;
		}
		return true;
	}

	private static bool TrySolveQuadratic(float a, float b, float c, out Vector2 res) {
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