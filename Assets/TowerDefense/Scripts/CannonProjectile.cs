using UnityEngine;
using System.Collections;

public class CannonProjectile : MonoBehaviour {
	public float m_speed = 0.2f;
	public byte m_damage = 10;

	void Update () {
		var translation = transform.forward * m_speed;
		transform.Translate (translation);
	}

	void OnTriggerEnter(Collider other) {
		Destroy (gameObject);
		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

	}
}
