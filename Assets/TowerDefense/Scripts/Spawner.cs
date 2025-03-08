using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
	public Monster monster;
	public float interval = 3;
	public List<Transform> moveTargets;

	private float m_lastSpawn = -1;
	private Transform m_tr;
	private ObjectPool<Monster> m_monsters;

	private void Awake() {
		m_tr = transform;
		m_monsters = new ObjectPool<Monster>(
			createFunc: () => {
				var m = Instantiate(monster);
				m.moveTargets = moveTargets;
				return m;
			},
			actionOnRelease: m => {
				m.gameObject.SetActive(false);
			},
			actionOnGet: m => {
				m.gameObject.SetActive(true);
			}
		);
	}

	public void Spawn() {
		var m = m_monsters.Get();
		m.transform.position = m_tr.position;
		m.Finished += () => m_monsters.Release(m);
		m.Move();
	}

	private void Update () {
		if (Time.time > m_lastSpawn + interval) {
			Spawn();
			m_lastSpawn = Time.time;
		}
	}
}