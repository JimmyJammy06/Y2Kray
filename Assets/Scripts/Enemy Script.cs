using UnityEngine;
using UnityEngine.AI;

public class EnemyScript: MonoBehaviour
{
    #region
    [SerializeField] protected EnemySO m_enemy;
    private float m_health;

    [SerializeField] private GameObject m_gameObject;
    [SerializeField] private Transform m_rightArm;
    [SerializeField] WeaponScript m_rightWeapon;    //Allows for dual wielding combos
    [SerializeField] bool m_isUsingLeftArm; //Because some enemies like the glass guy might have only 1 weapon, or might have their arms be disabled
    [SerializeField] private Transform m_leftArm;
    [SerializeField] WeaponScript m_leftWeapon;    //Allows for dual wielding combos
    private Transform m_transform;
    //private Rigidbody m_rigid;
    private NavMeshAgent m_agent;

    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private Collider m_detectionSphere;
    private bool m_detected;
    private float m_damageImmuneTimer;
    private float m_waitTimer = 3;
    private float m_comboTimer;
    private float m_comboPlace;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_health = m_enemy.Health;
        m_transform = m_gameObject.GetComponent<Transform>();
        //m_rigid = m_gameObject.GetComponent<Rigidbody>();
        m_agent = m_gameObject.GetComponent<NavMeshAgent>();
        m_detected = m_enemy.Follow;   //some enemies wil immediately follow the player, like bosses or those seen in cutscenes
        m_damageImmuneTimer = 0;
    }

    void Update()
    {
        if (m_detected)
        {
            m_agent.destination = m_playerTransform.position;
            if (InRange(m_playerTransform))
            {
                //hit player
                if (m_waitTimer > 0)
                {
                    m_waitTimer -= Time.deltaTime;
                    if (m_waitTimer <= 0)
                    {
                        m_waitTimer = 0;
                        Resting();
                    }
                }
                else
                {
                    Light_Attack();
                }
            }
            if (m_damageImmuneTimer > 0)
            {
                m_damageImmuneTimer -= Time.deltaTime;
            }
            if (m_comboTimer > 0)
            {
                m_comboTimer -= Time.deltaTime;
                if (m_comboTimer <= 0)
                {
                    if (m_comboTimer <= 0)
                    {
                        m_comboTimer = 0;
                        m_comboPlace = 0;
                    }
                }
            }

        }
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.CompareTag("Light Attack") && m_damageImmuneTimer <=0)
        {
            float damage = collider.GetComponentInParent<WeaponScript>().GetPower();
            m_health -= damage;
            m_damageImmuneTimer = 0.1f; // To prevent damage from happening multiple times on colliders
        }
        else if (collider.CompareTag("Heavy Attack") && m_damageImmuneTimer <= 0)
        {
            float damage = collider.GetComponentInParent<WeaponScript>().GetPower();
            m_health -= damage * 1.5f;
            m_damageImmuneTimer = 0.1f;
        }
        if (m_health <= 0)
        {
            Destroy(m_gameObject);
        }
    }

    //private void OnTriggerExit(Collider collider)
    //{
    //    if (collider.tag == "Player" && !m_enemy.Follow)
    //    {
    //        m_detected = false;
    //    }
    //}

    private bool InRange(Transform target)
    {
        if (Mathf.Abs(m_transform.position.x - target.position.x) < m_rightWeapon.GetRange())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RightAttack()
    {
        m_rightArm.localPosition = new Vector3(0.6f, 0.17f, 0.72233f);
        m_rightArm.rotation = Quaternion.Euler(90, 180, -40);
    }

    private void LeftAttack()
    {
        m_leftArm.localPosition = new Vector3(0.16f, 0.06f, 1.27f);
        m_leftArm.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void Resting()
    {
        m_rightArm.localPosition = new Vector3(0.6f, 0.19f, 0.73f);
        m_rightArm.rotation = Quaternion.Euler(-4, -88, -7.745f);
        if (m_isUsingLeftArm)
        {
        m_leftArm.localPosition = new Vector3(-0.6f, 0.19f, 0.73f);
        m_leftArm.rotation = Quaternion.Euler(16, -90, -26.5f);
        }
    }

    public void Light_Attack()
    {
        switch (m_comboPlace)
        {
            case 0:
                {
                    m_waitTimer = m_rightWeapon.GetSpeed();
                    RightAttack();
                    StartCoroutine(m_rightWeapon.Activate_Light_Hitboxs(1));
                    m_comboPlace++;
                    m_comboTimer = 3.0f;
                    break;
                }
            case 1:
                {
                    if (m_isUsingLeftArm)
                    {
                        m_waitTimer = m_rightWeapon.GetSpeed();
                        RightAttack();
                        StartCoroutine(m_rightWeapon.Activate_Light_Hitboxs(2));
                    }
                    else
                    {
                        m_waitTimer = m_leftWeapon.GetSpeed();
                        LeftAttack();
                        StartCoroutine(m_leftWeapon.Activate_Light_Hitboxs(2));
                    }
                    m_comboPlace++;
                    m_comboTimer = 3.0f;
                    break;
                }
            case 2:
                {
                    m_waitTimer = m_rightWeapon.GetSpeed();
                    RightAttack();
                    StartCoroutine(m_rightWeapon.Activate_Light_Hitboxs(3));
                    m_comboPlace = 0;
                    break;
                }
            default:
                { break; }
        }
        
    }

    public void SetDetection(bool val)
    {
        m_detected = val;
    }
}
