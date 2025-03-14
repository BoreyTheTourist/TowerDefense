using UnityEngine;

public class Spawner : MonoBehaviour {
	public Transform[] moveTargets;
	
	private Transform m_tr;

	private void Awake() {
		m_tr = transform;
		if (moveTargets.Length == 0) {
			moveTargets = new Transform[m_tr.childCount];
			byte i = 0;
			foreach (Transform ch in m_tr) {
				moveTargets[i++] = ch;
			}
		}
	}

	public void Spawn(Monster monster) {
		var m = Instantiate(monster, m_tr);
		m.Finished += () => Destroy(m.gameObject);
		m.SetStops(moveTargets);
		m.Move();
	}
}