using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Prefab Library", fileName = "PrefabLibrary")]
public sealed class CarLibrary : ScriptableObject
{
    [SerializeField] private GameObject[] prefabs;
    public IReadOnlyList<GameObject> Prefabs => prefabs;
}
