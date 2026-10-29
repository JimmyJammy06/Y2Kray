using UnityEngine;

public class DetectioncheckerScript : MonoBehaviour
{
    [SerializeField] private EnemyScript m_EnemyScript;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            m_EnemyScript.SetDetection(true);
        }
    }
}
