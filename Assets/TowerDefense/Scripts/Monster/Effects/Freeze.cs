using UnityEngine;

public class Freeze : MonoBehaviour {
	[SerializeField] private Driver m_driver;
	public float rate = 1f;

	private void Awake() {
		if (m_driver == null) {
			m_driver = GetComponentInParent<Driver>();
		}
	}

	public void Apply(float amount) {
		m_driver.ChangeSpeedBy(amount * rate);
	}

	public void Remove(float amount) {
		m_driver.ChangeSpeedBy(1 / (amount * rate));
	}
}