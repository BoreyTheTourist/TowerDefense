using System.Collections;
using UnityEngine;

public class ProjectileTowerController : MonoBehaviour {
	[SerializeField] private Tower m_tower;
	[SerializeField] private ProjectileCannon m_cannon;
	public float shootDelay = .5f;

	private float m_timer;
	private Transform m_lastTarget;

	private void Start() {
		if (m_tower == null) {
			m_tower = GetComponentInChildren<Tower>();
		}
		if (m_cannon == null) {
			m_cannon = GetComponentInChildren<ProjectileCannon>();
		}
	}

	private void FixedUpdate() {
		if (!m_tower.TryGetFirstTarget(out var target)) {
			return;
		}
		if (target != m_lastTarget) {
			m_cannon.SetTarget(target);
			m_lastTarget = target;
		}
		if (m_timer + shootDelay <= Time.time) {
			m_timer = Time.time;
			m_cannon.Shoot();
		}
	}
}