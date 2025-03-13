using System.Collections;
using UnityEngine;

public class Driver : MonoBehaviour {
	public Transform[] stops;
	public Vector3 velocity { get; private set; }
	[SerializeField] private float m_speed = 2f;

	public event System.Action Arrived;

	private Transform m_tr;
	private float m_speed2;

	private void Awake() {
		m_tr = transform;
		m_speed2 = m_speed * m_speed;
	}

	public void StartDrive() {
		StartCoroutine(DriveCor());
	}

	private IEnumerator DriveCor() {
		foreach (var s in stops) {
			var dir = (s.position - m_tr.position).normalized;
			m_tr.rotation = Quaternion.LookRotation(dir);
			velocity = dir * m_speed;
			var v = velocity * Time.fixedDeltaTime;
			while (Vector3.SqrMagnitude(m_tr.position - s.position) > m_speed2) {
				m_tr.position += v;
				yield return new WaitForFixedUpdate();
			}
		}
		Arrived?.Invoke();
	}
}