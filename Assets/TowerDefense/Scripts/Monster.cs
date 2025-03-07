using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	public GameObject m_moveTarget;
	public float m_speed = 0.1f;
	public int m_maxHP = 30;
	const float m_reachDistance = 0.3f;

	public int m_hp;

	void Start() {
		m_hp = m_maxHP;
	}

	void Update () {
		if (m_moveTarget == null)
			return;
		
		if (Vector3.Distance (transform.position, m_moveTarget.transform.position) <= m_reachDistance) {
			Destroy (gameObject);
			return;
		}

		var translation = m_moveTarget.transform.position - transform.position;
		if (translation.magnitude > m_speed) {
			translation = translation.normalized * m_speed;
		}
		transform.Translate (translation);
	}
}
