using UnityEngine;
using System.Collections;

public class GuidedProjectile : MonoBehaviour {
	public GameObject m_target;
	public float m_speed = 0.2f;
	public int m_damage = 10;

	void Update () {
		if (m_target == null) {
			Destroy (gameObject);
			return;
		}

		var translation = m_target.transform.position - transform.position;
		if (translation.magnitude > m_speed) {
			translation = translation.normalized * m_speed;
		}
		transform.Translate (translation);
	}

	void OnTriggerEnter(Collider other) {
		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

		monster.m_hp -= m_damage;
		if (monster.m_hp <= 0) {
			Destroy (monster.gameObject);
		}
		Destroy (gameObject);
	}
}
