using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] public WeaponSO m_weapon;
    [SerializeField] private GameObject m_tonfaStriker;
    [SerializeField] private GameObject[] m_lightAttacks;
    [SerializeField] private GameObject[] m_heavyAttacks;
    private bool m_flipped = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName()
    {
        return m_weapon.Type;
    }
    public float GetPower()
    {
        return m_weapon.Power;
    }
    public float GetSpeed()
    {
        return m_weapon.Speed;
    }
    public float GetRange()
    {
        return m_weapon.Range;
    }

    public IEnumerator Activate_Light_Hitboxs(int num)
    {
        if (m_flipped && m_weapon.flippable)
        {
            Flip(); //reset position for light attack
        }
        switch (num)
        {
            case 1:
                {
                    m_lightAttacks[0].SetActive(true);
                    yield return new WaitForSeconds(m_weapon.Speed);
                    m_lightAttacks[0].SetActive(false);
                    break;
                }
            case 2:
                {
                    m_lightAttacks[1].SetActive(true);
                    yield return new WaitForSeconds(m_weapon.Speed);
                    m_lightAttacks[1].SetActive(false);
                    break;
                }
            case 3:
                {
                    m_lightAttacks[2].SetActive(true);
                    yield return new WaitForSeconds(m_weapon.Speed);
                    m_lightAttacks[2].SetActive(false);
                    break;
                }
        }
    }
    public IEnumerator Activate_Heavy_Hitboxs(int num)
    {
        if (!m_flipped && m_weapon.flippable)
        {
            Flip();
        }
        switch (num)
        {
            case 0:
                {
                    m_heavyAttacks[0].SetActive(true);
                    yield return new WaitForSeconds(m_weapon.Speed * 1.5f);
                    m_heavyAttacks[0].SetActive(false);
                    break;
                }
            case 1:
                {
                    m_heavyAttacks[1].SetActive(true);
                    yield return new WaitForSeconds(m_weapon.Speed * 1.5f);
                    m_heavyAttacks[1].SetActive(false);
                    break;
                }
            case 2:
                {
                    m_heavyAttacks[2].SetActive(true);
                    yield return new WaitForSeconds(m_weapon.Speed * 1.5f);
                    m_heavyAttacks[2].SetActive(false);
                    break;
                }
        }
    }

    private void Flip()
    {
        if (m_flipped)    //Return to guard position
        {
            m_tonfaStriker.transform.localPosition = new Vector3(-0.18f, -0.25f, 0);
            m_flipped = false;
        }
        else //Turn weapon to increase range
        {
            m_tonfaStriker.transform.localPosition = new Vector3(-0.18f, 0.228f, 0);
            m_flipped = true;
        }
    }

}
