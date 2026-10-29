using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerScript : MonoBehaviour
{
    [SerializeField] protected GameObject m_speakerSound;
    [SerializeField] protected Collider m_zone;
    protected bool m_inZone = false;

    [SerializeField] protected GameObject[] m_dialogue;

    [SerializeField] protected GameObject m_speakerPanel;
    [SerializeField] protected int m_timeToHack = 5; //Varied based on level or boss status
    [SerializeField] protected int m_timeToHackLossMod = 2;   //Can be upgraded maybe?
    [SerializeField] protected Image m_hackProgress;
    protected float m_hackProgressFlt = 0;
    protected bool m_complete = false;

    [SerializeField] private PenultimateSetup m_penultimateSpeaker;
    // Update is called once per frame
    void Update()
    {
        if (!m_complete && m_inZone)
        {
            m_hackProgressFlt += Time.deltaTime;
            m_hackProgress.fillAmount = m_hackProgressFlt / m_timeToHack;    //Should fill in 5 seconds
            if (m_hackProgressFlt >= m_timeToHack)
            {
                m_complete = true;
                m_zone.enabled = false; //Disable collider to prevent the process from running in the backrgound
                m_inZone = false;
                m_speakerPanel.SetActive(false);
                m_penultimateSpeaker.ReportCompletion();
                StopCoroutines();
                if (m_penultimateSpeaker.IsLastSpeaker())   // Tell the player the next one is the boss, will take more time
                {
                    StartCoroutine(BossNextTalk());
                }
                else   // Generic tell the player to move to next
                {
                    StartCoroutine(CompleteTalk()); 
                }

                m_speakerSound.SetActive(true);
            }
        }
        else if (!m_complete && !m_inZone && m_hackProgressFlt > 0)
        {
            m_hackProgressFlt -= Time.deltaTime/ m_timeToHackLossMod;
            m_hackProgress.fillAmount = m_hackProgressFlt / m_timeToHack;
            if (m_hackProgressFlt < 0)
            {
                m_hackProgressFlt = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            m_inZone = true;
            m_speakerPanel.SetActive(true);
            StopCoroutines();
            StartCoroutine(EnterTalk());
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            m_inZone = false;
            m_speakerPanel.SetActive(false);
            StopCoroutines(); 
            StartCoroutine(LeaveTalk());
        }
    }

    protected IEnumerator EnterTalk()
    {
        m_dialogue[2].SetActive(false); // Just incase any other text is blocking the screen from any other interactions
        m_dialogue[3].SetActive(false); 
        m_dialogue[0].SetActive(true);
        m_dialogue[1].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        m_dialogue[0].SetActive(false); 
        m_dialogue[1].SetActive(false);
    }
    protected IEnumerator LeaveTalk()
    {
        m_dialogue[1].SetActive(false); // Just incase any other text is blocking the screen from any other interactions
        m_dialogue[3].SetActive(false);
        m_dialogue[0].SetActive(true);
        m_dialogue[2].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        m_dialogue[0].SetActive(false);
        m_dialogue[2].SetActive(false);
}
    protected IEnumerator CompleteTalk()
    {
        m_dialogue[1].SetActive(false); // Just incase any other text is blocking the screen from any other interactions
        m_dialogue[2].SetActive(false);
        m_dialogue[4].SetActive(false);
        m_dialogue[0].SetActive(true);
        m_dialogue[3].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        m_dialogue[0].SetActive(false);
        m_dialogue[3].SetActive(false);
    }
    IEnumerator BossNextTalk()
    {
        m_dialogue[1].SetActive(false);
        m_dialogue[2].SetActive(false);
        m_dialogue[3].SetActive(false);
        m_dialogue[0].SetActive(true);
        m_dialogue[4].SetActive(true);
        yield return new WaitForSecondsRealtime(4);
        m_dialogue[0].SetActive(false);
        m_dialogue[4].SetActive(false);
    }

    protected void StopCoroutines()
    {
        StopCoroutine(EnterTalk());
        StopCoroutine(LeaveTalk());
        StopCoroutine(CompleteTalk());
        StopCoroutine(BossNextTalk());
    }

    public bool GetCompleted()
    {
        return m_complete;
    }
}
