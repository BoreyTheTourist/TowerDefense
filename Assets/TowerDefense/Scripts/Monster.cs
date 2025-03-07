﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour {
	[Header("Move")]
	public List<Transform> moveTargets;
	[SerializeField] public Rigidbody m_body;
	public Vector3 speed { get => m_body.linearVelocity; }
	public float speedVal = 0.1f;

	[Header("Damageable")]
	[SerializeField] private Damageable m_dmg;

	public event System.Action Finished;

	private float m_speed2;
	private Transform m_tr;

	private void Awake() {
		m_speed2 = speedVal * speedVal;
	}

	private void OnEnable() {
		if (m_tr == null) {
			m_tr = transform;
		}
		if (m_body == null) {
			m_body = GetComponent<Rigidbody>();
		}
		if (m_dmg == null) {
			m_dmg = GetComponentInChildren<Damageable>();
		}
		m_dmg.Died += Finish;
	}

	private void OnDisable() {
		CancelInvoke();
		StopAllCoroutines();
		m_body.linearVelocity = Vector3.zero;
		m_dmg.Died -= Finish;
		Finished = null;
	}

	public void Move() {
		StartCoroutine(MoveCor());
	}

	private IEnumerator MoveCor() {
		foreach (var target in moveTargets) {
			var dir = (target.position - m_tr.position).normalized;
			m_tr.rotation = Quaternion.LookRotation(dir);
			m_body.linearVelocity = dir * speedVal;
			while (Vector3.SqrMagnitude(m_tr.position - target.position) > m_speed2) {
				yield return new WaitForFixedUpdate();
			}
		}
		Finished?.Invoke();
	}

	private void Finish() {
		Finished?.Invoke();
	}
}