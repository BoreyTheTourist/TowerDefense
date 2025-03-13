using UnityEngine;

public class Damageable : MonoBehaviour {
	[SerializeField] private float m_maxHp = 30;
	public float maxHp { get => m_maxHp; }
	public float hp { get; private set; }

	public event System.Action Died;

	private void OnEnable() {
		hp = m_maxHp;
	}

	public void TakeDamage(float amount) {
		if (hp <= 0) {
			return;
		}
		if ((hp -= amount) <= 0) {
			Died?.Invoke();
		}
	}
}