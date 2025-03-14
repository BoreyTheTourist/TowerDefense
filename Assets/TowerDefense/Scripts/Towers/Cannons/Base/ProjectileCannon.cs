using UnityEngine;

public abstract class ProjectileCannon : MonoBehaviour {
	public abstract void SetTarget(Transform target);
	public abstract void ResetCurTarget();
	public abstract void Shoot();
}