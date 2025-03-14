using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tower : MonoBehaviour {
	public event System.Action<Transform> TargetEntered;
	public event System.Action<Transform> TargetExited;

	private LinkedList<Transform> m_targets = new LinkedList<Transform>();

	private void Start() {
		var col = GetComponent<Collider>();
		col.isTrigger = true;
	}

	public bool TryGetFirstTarget(out Transform target) {
		if (m_targets.Count > 0) {
			target = m_targets.First.Value;
			return true;
		}
		target = null;
		return false;
	}

	private void OnTriggerEnter(Collider other) {
		var t = other.transform;
		var dmg = t.GetComponentInParent<Damageable>();
		if (dmg != null) {
			dmg.Died += () => m_targets.Remove(t);
		}
		m_targets.AddLast(t);
		TargetEntered?.Invoke(t);
	}

	private void OnTriggerExit(Collider other) {
		var t = other.transform;
		m_targets.Remove(t);
		TargetExited?.Invoke(t);
	}
}