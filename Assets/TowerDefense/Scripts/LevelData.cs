using UnityEngine;

[System.Serializable]
public struct WaveData {
	public Monster monster;
	public byte count;
	public float initialDelay, spawnDelay;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject {
	public WaveData[] waves;
}