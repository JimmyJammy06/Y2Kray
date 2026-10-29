using Unity.Cinemachine;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private CinemachineCamera m_camera;
    [SerializeField] private Transform m_player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraLockOn(Transform lockTo)
    {
        m_camera.LookAt = lockTo;
        m_camera.Follow = lockTo;
    }
    public void FightLockOn(Transform lockTo)
    {
        m_camera.LookAt = lockTo;
    }

    public void Return2Player()
    {
        m_camera.LookAt = m_player;
        m_camera.Follow = m_player;
    }
}
