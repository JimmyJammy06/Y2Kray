using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class navreset : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        surface.BuildNavMesh();
    }
}
