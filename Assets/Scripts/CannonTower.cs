using UnityEngine;
using System.Collections;

public class CannonTower : MonoBehaviour {
	public float m_shootInterval = 0.5f;
	public float m_range = 4f;
	public GameObject m_projectilePrefab;
	public Transform m_shootPoint;

	private float m_lastShotTime = -0.5f;

	void Update () {
		if (m_projectilePrefab == null || m_shootPoint == null)
			return;

		foreach (var monster in FindObjectsOfType<Monster>()) {
			if (Vector3.Distance (transform.position, monster.transform.position) > m_range)
				continue;

			if (m_lastShotTime + m_shootInterval > Time.time)
				continue;

			// shot
			Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);

			m_lastShotTime = Time.time;
		}

	}
}
