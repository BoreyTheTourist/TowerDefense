using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour {
	public LevelData data;
	public Spawner spawner;

	private void Awake() {
		if (spawner == null) {
			FindAnyObjectByType<Spawner>();
		}
	}

	private void Start() {
		StartCoroutine(Generate());
	}

	private IEnumerator Generate() {
		foreach (var wd in data.waves) {
			yield return new WaitForSeconds(wd.initialDelay);
			for (int i = 0; i < wd.count; ++i) {
				spawner.Spawn(wd.monster);
				yield return new WaitForSeconds(wd.spawnDelay);
			}
		}
	}
}