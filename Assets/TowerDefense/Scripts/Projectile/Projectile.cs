using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public static class ProjectilePool {
	private static Dictionary<System.Type, ObjectPool<Projectile>> m_dict = new
		Dictionary<System.Type, ObjectPool<Projectile>>();

	public static T Get<T>() where T : Projectile {
		return m_dict[typeof(T)].Get() as T;
	}

	public static void Release<T>(T proj) where T : Projectile {
		m_dict[typeof(T)].Release(proj);
	}

	public static void AddPool<T>(T prefab) where T : Projectile {
		if (m_dict.ContainsKey(typeof(T))) {
			return;
		}
		var pool = new ObjectPool<Projectile>(
			createFunc: () => {
				return Object.Instantiate(prefab);
			},
			actionOnGet: p => {
				p.gameObject.SetActive(true);
			},
			actionOnRelease: p => {
				p.gameObject.SetActive(false);
				p.Release();
			},
			actionOnDestroy: p => {
				Object.Destroy(p.gameObject);
			}
		);
		m_dict.Add(typeof(T), pool);
	}
}

[RequireComponent(typeof(Collider))]
public abstract class Projectile : MonoBehaviour {
	[SerializeField] protected float m_damage;
	public event System.Action Destroyed;

	private void Start() {
		var col = GetComponent<Collider>();
		col.isTrigger = true;
	}

	private void OnTriggerEnter(Collider other) {
		var dmg = other.GetComponentInParent<Damageable>();
		if (dmg != null) {
			dmg.TakeDamage(m_damage);
		}
		Destroyed?.Invoke();
	}

	public virtual void Release() {
		CancelInvoke();
		Destroyed = null;
	}

	protected void OnDestroyed() {
		Destroyed?.Invoke();
	}
}