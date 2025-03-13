using UnityEngine;

public class EffectCannonController : MonoBehaviour {
	[SerializeField] private Tower m_tower;
	[SerializeField] private EffectCannon m_cannon;

	private void Start() {
		if (m_tower == null) {
			m_tower = GetComponentInChildren<Tower>();
		}
		if (m_cannon == null) {
			m_cannon = GetComponentInChildren<EffectCannon>();
		}
		m_tower.TargetEntered += t => m_cannon.Apply(t);
		m_tower.TargetExited += t => m_cannon.Remove(t);
	}
}