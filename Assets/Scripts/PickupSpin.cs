using UnityEngine;

public class PickupSpin : MonoBehaviour
{
    [SerializeField] private Transform m_transform;
    [SerializeField] private Transform m_from;
    [SerializeField] private Transform m_to;
    private float m_rotationSpeed = 6;
    private float m_current = 0.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(m_transform.rotation, m_to.rotation, m_current);
        m_current += m_rotationSpeed * Time.deltaTime;
        //Debug.Log(m_current);
        if (m_current >= 360)
        {
            m_current = 0;
        }
    }
}
