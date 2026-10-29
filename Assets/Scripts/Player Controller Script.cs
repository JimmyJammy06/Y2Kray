using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerScript : MonoBehaviour
{
    #region
    [SerializeField] private GameObject m_player;
    private Rigidbody m_rigidbody;

    [SerializeField] private Music m_musicManager;

    private Vector3 m_movementDirection;
    [SerializeField] private float m_movementSpeed;
    //private float m_currentSpeed;

    [SerializeField] Transform m_rightArm;
    [SerializeField] WeaponScript m_rightWeapon;    //Allows for dual wielding combos
    [SerializeField] Transform m_leftArm;
    [SerializeField] WeaponScript m_leftWeapon;    //Allows for dual wielding combos


    //[SerializeField] WeaponScript m_leftWeapon = null;
    private float m_damageImmuneTimer = 0;
    private float m_waitTimer;
    private float m_comboTimer;
    private float m_comboPlace;

    private float m_mouseX;
    //private float m_lastMouseY;
    private float m_camX;
    [SerializeField] private Transform m_cam;
    

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody = m_player.GetComponent<Rigidbody>();
        m_camX = Mathf.Clamp(0, -15, 30);

        //
        //transform.Rotate()
    }

    // Update is called once per frame
    void Update()
    {

        //transform.forward
        m_rigidbody.linearVelocity = Vector3.zero;
        m_rigidbody.linearVelocity += m_player.transform.forward * m_movementDirection.z * m_movementSpeed;
        m_rigidbody.linearVelocity += m_player.transform.right * m_movementDirection.x * m_movementSpeed;
        if (m_comboTimer > 0)
        {
            m_comboTimer-= Time.deltaTime;
            if (m_comboTimer <= 0)
            {
                if (m_comboTimer <= 0)
                {
                    m_comboTimer = 0;
                    m_comboPlace = 0;
                }
            }
        }
        if (m_damageImmuneTimer > 0)
        {
            m_damageImmuneTimer -= Time.deltaTime;
        }
        if (m_waitTimer > 0)
        {
            m_waitTimer-= Time.deltaTime;
            if (m_waitTimer <= 0)
            {
                m_waitTimer = 0;
                Resting();
            }
        }
    }

    public void Handle_Move(InputAction.CallbackContext ctx)
    {
        //func enter
        if (ctx.performed)
        {
            //perform
            m_movementDirection = ctx.ReadValue<Vector3>().normalized;
            //Debug.Log(m_movementDirection);
            //if (m_movementDirection.z > 0)
            //{
            //    m_rigidbody.linearVelocity = m_player.transform.forward * m_movementSpeed;
            //}
            //else if (m_movementDirection.z < 0)
            //{
            //    m_rigidbody.linearVelocity = m_player.transform.forward * -m_movementSpeed;
            //}
        }
        else if (ctx.canceled)
        {
            // reset movement vector
            m_movementDirection = Vector3.zero;
        }
    }

    public void Handle_Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            m_musicManager.Music_ONOFF();
        }
    }

    public void Handle_Look(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            m_mouseX = ctx.ReadValue<Vector2>().x;
            //m_mouseX = (m_mouseX / 450) - 1;  //put this value into a range between -1 and 1
            //Debug.Log(m_mouseX* 0.4 + 180);
            m_player.transform.rotation = Quaternion.Euler(m_player.transform.rotation.x, m_mouseX * 0.4f + 180, m_player.transform.rotation.z);    //Rotates the characters Y to the mouses X position while keeping the players X and Z rotation constant

            //if (m_lastMouseY != ctx.ReadValue<Vector2>().x)
            //{
            //    if (m_lastMouseY < ctx.ReadValue<Vector2>().x)
            //    {
            //        m_camX -= 0.1f;
            //    }
            //    else
            //    {
            //        m_camX += 0.1f;
            //    }
            //    m_cam.rotation = Quaternion.Euler(m_camX, m_cam.rotation.y, m_cam.rotation.z);    //Rotates the characters Y to the mouses X position while keeping the players X and Z rotation constant

            //}
        }
    }
    
    public void Handle_Light_Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && m_waitTimer <= 0)
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
                        m_waitTimer = m_leftWeapon.GetSpeed();
                        LeftAttack();
                        StartCoroutine(m_leftWeapon.Activate_Light_Hitboxs(2));
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
    }

    public void Handle_Heavy_Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && m_waitTimer <= 0)
        {
            switch (m_comboPlace)
            {
                case 0:
                {
                        m_waitTimer = m_rightWeapon.GetSpeed();
                        RightAttack();
                        StartCoroutine(m_rightWeapon.Activate_Heavy_Hitboxs(1));
                        m_comboPlace++;
                        m_comboTimer = 3.0f;
                        break;
                }
                case 1:
                {
                        m_waitTimer = m_leftWeapon.GetSpeed();
                        LeftAttack();
                        StartCoroutine(m_leftWeapon.Activate_Heavy_Hitboxs(2));
                        m_comboPlace++;
                        m_comboTimer = 3.0f;
                        break;
                }
                case 2:
                {
                        m_waitTimer = m_rightWeapon.GetSpeed();
                        RightAttack();
                        StartCoroutine(m_rightWeapon.Activate_Heavy_Hitboxs(3));
                        m_comboPlace = 0;
                        break;
                }
                default:
                    { break; }
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("collision");
    //    if (collision.gameObject.tag == "Battery")
    //    {
    //        Debug.Log("thing happen");
    //        m_musicManager.GetBattery();
    //    }
    //}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "SafeZone")
        {
            m_musicManager.Set_SafeZone(true);
        }
        else if (collision.tag == "Enemy Light Attack" && m_damageImmuneTimer <= 0)
        {
            float damage = collision.GetComponentInParent<WeaponScript>().GetPower();
            m_musicManager.Hit(damage);
            m_damageImmuneTimer = 0.1f; // To prevent damage from happening multiple times on colliders
        }
        else if (collision.gameObject.tag == "Battery")
        {
            Debug.Log("thing happen");
            Destroy(collision.gameObject);
            m_musicManager.GetBattery();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "SafeZone")
        {
            m_musicManager.Set_SafeZone(false);
        }
    }

    private void RightAttack()
    {
        m_rightArm.localPosition = new Vector3(0.16f, 0.06f, 1.27f);
        m_rightArm.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void LeftAttack()
    {
        m_leftArm.localPosition = new Vector3(0.16f, 0.06f, 1.27f);
        m_leftArm.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void Resting()
    {
        m_rightArm.localPosition = new Vector3(0.6f, 0.19f, 0.73f);
        m_rightArm.rotation = Quaternion.Euler(-12, -80, -26.5f);
        m_leftArm.localPosition = new Vector3(-0.6f, 0.19f, 0.73f);
        m_leftArm.rotation = Quaternion.Euler(16, -90, -26.5f);
    }

    private void Guard()
    {
        m_rightArm.localPosition = new Vector3(0.24f, 0.37f, 0.92f);
        m_rightArm.rotation = Quaternion.Euler(18.404f, -84f, -0.15f);
        m_leftArm.localPosition = new Vector3(-0.3f, 0.383f, 0.877f);
        m_leftArm.rotation = Quaternion.Euler(0, 0, -22);
    }
}
