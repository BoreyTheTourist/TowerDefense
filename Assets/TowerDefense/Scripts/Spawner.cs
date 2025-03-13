using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
	public Monster monster;
	public float interval = 3;
	public Transform[] moveTargets;

	private float m_lastSpawn = -1;
	private Transform m_tr;

	private void Awake() {
		m_tr = transform;
	}

	public void Spawn() {
		var m = Instantiate(monster, m_tr);
		m.Finished += () => Destroy(m.gameObject);
		m.SetStops(moveTargets);
		m.Move();
	}

	private void Update () {
		if (Time.time > m_lastSpawn + interval) {
			Spawn();
			m_lastSpawn = Time.time;
		}
	}
}