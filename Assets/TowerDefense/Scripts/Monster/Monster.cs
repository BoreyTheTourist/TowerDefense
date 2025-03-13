using UnityEngine;

public class Monster : MonoBehaviour {
	[SerializeField] private Driver m_driver;
	[SerializeField] private Damageable m_dmg;

	public event System.Action Finished;

	public void SetStops(Transform[] stops) {
		if (m_driver) {
			m_driver.stops = stops;
		}
	}

	public void Move() {
		if (m_driver) {
			m_driver.StartDrive();
		}
	}

	private void OnEnable() {
		m_dmg.Died += Finish;
		m_driver.Arrived += Finish;
	}

	private void Finish() {
		Finished?.Invoke();
	}
}