using UnityEngine;

public abstract class EffectCannon : MonoBehaviour {
	public abstract void Apply(Transform target);
	public abstract void Remove(Transform target);
}