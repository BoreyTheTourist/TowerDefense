using UnityEngine;

public class FreezeEffectCannon : EffectCannon {
	public float slowRate = .3f;

	public override void Apply(Transform target) {
		if (target.TryGetComponent<Freeze>(out var f)) {
			f.Apply(slowRate);
		}
	}

	public override void Remove(Transform target) {
		if (target.TryGetComponent<Freeze>(out var f)) {
			f.Remove(slowRate);
		}
	}
}