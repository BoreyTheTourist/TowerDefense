using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tower : MonoBehaviour {
	public event System.Action<Transform> TargetEntered;
	public event System.Action<Transform> TargetExited;

	private Queue<Transform> m_targets = new Queue<Transform>();

	private void Start() {
		var col = GetComponent<Collider>();
		col.isTrigger = true;
	}

	// not event 'cause need to clear
	public bool TryGetFirstTarget(out Transform target) {
		while (m_targets.TryPeek(out target)) {
			if (target != null) {
				return true;
			}
			m_targets.Dequeue();
		}
		return false;
	}

	private void OnTriggerEnter(Collider other) {
		var t = other.transform;
		m_targets.Enqueue(t);
		TargetEntered?.Invoke(t);
	}

	private void OnTriggerExit(Collider other) {
		TargetExited?.Invoke(m_targets.Dequeue());
	}
}