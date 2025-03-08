using UnityEngine;
using System.Collections;

public class GuidedProjectile : MonoBehaviour {
	public GameObject m_target;
	public float m_speed = 0.2f;
	public byte m_damage = 10;

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
		Destroy (gameObject);
		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;
	}
}