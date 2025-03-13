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
		return m_targets.TryPeek(out target);
	}

	private void OnTriggerEnter(Collider other) {
		Clean();
		var t = other.transform;
		m_targets.Enqueue(t);
		TargetEntered?.Invoke(t);
	}

	private void OnTriggerExit(Collider other) {
		Clean();
		var t = m_targets.Dequeue();
		TargetExited?.Invoke(t);
	}

	private void Clean() {
		while (m_targets.TryPeek(out var t)) {
			if (t != null) {
				return;
			}
			m_targets.Dequeue();
		}
	}
}