using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSO", menuName = "Scriptable Objects/SpawnSO")]
public class SpawnSO : ScriptableObject
{
    public string prefabName;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
}

