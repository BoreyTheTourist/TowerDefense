using UnityEngine;
using System.Collections;

public class SimpleTower : MonoBehaviour {
	public float m_shootInterval = 0.5f;
	public float m_range = 4f;
	public GameObject m_projectilePrefab;

	private float m_lastShotTime = -0.5f;
	
	void Update () {
		if (m_projectilePrefab == null)
			return;

		foreach (var monster in FindObjectsOfType<Monster>()) {
			if (Vector3.Distance (transform.position, monster.transform.position) > m_range)
				continue;

			if (m_lastShotTime + m_shootInterval > Time.time)
				continue;

			// shot
			var projectile = Instantiate(m_projectilePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity) as GameObject;
			var projectileBeh = projectile.GetComponent<GuidedProjectile> ();
			projectileBeh.m_target = monster.gameObject;

			m_lastShotTime = Time.time;
		}
	
	}
}
