using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PenultimateSetup : MonoBehaviour
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
    [SerializeField] private DialogueScript m_dialoguescript;
    [SerializeField] private Music m_musicScript;

    private int m_speakersComplete = 0;
    [SerializeField] private int m_speakersNeeded;
    [SerializeField] private GameObject[] m_damageWaves;
    [SerializeField] private GameObject[] m_damageWaveWarnings;
    private float m_attackTimer = 1;

    void Update()
    {
        if (m_attackTimer > 0 &&!m_complete)
        {
            m_attackTimer -= Time.deltaTime;
            if (m_attackTimer <= 0 && !m_complete)
            {
                StartCoroutine(SoundAttack());
                m_attackTimer = 10;
            }
        }
        if (!m_complete && m_inZone)
        {
            m_hackProgressFlt += Time.deltaTime;
            m_hackProgress.fillAmount = m_hackProgressFlt / m_timeToHack;    
            if (m_hackProgressFlt >= m_timeToHack)
            {
                m_complete = true;
                m_speakerPanel.SetActive(false);
                StopCoroutines();
                m_speakerSound.SetActive(true);
                m_dialoguescript.enabled = true;
                m_musicScript.ChangeMusic(false);
            }
        }
        else if (!m_complete && !m_inZone && m_hackProgressFlt > 0)
        {
            m_hackProgressFlt -= Time.deltaTime / m_timeToHackLossMod;
            m_hackProgress.fillAmount = m_hackProgressFlt / m_timeToHack;
            if (m_hackProgressFlt < 0)
            {
                m_hackProgressFlt = 0;
            }
        }
    }

    public void ReportCompletion()    
    {
        m_speakersComplete++;
        if (m_speakersComplete == m_speakersNeeded)
        {
            m_zone.enabled = true;
        }
    }

    public bool IsLastSpeaker()
    {
        if (m_speakersComplete == m_speakersNeeded)
        {
            return true;    // so instead of playing "Go to the next speaker" dialogue, we can instead play "The next speaker is the final one, it will be harder" etc etc
        }
        else
        {
            return false;
        }
    }



    IEnumerator SoundAttack()
    {
        m_damageWaveWarnings[0].SetActive(true);
        m_damageWaveWarnings[2].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        m_damageWaveWarnings[0].SetActive(false);
        m_damageWaveWarnings[2].SetActive(false);
        m_damageWaves[0].SetActive(true);
        m_damageWaves[2].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        m_damageWaves[0].SetActive(false);
        m_damageWaves[2].SetActive(false);
        m_damageWaveWarnings[1].SetActive(true);
        m_damageWaveWarnings[3].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        m_damageWaveWarnings[1].SetActive(false);
        m_damageWaveWarnings[3].SetActive(false);
        m_damageWaves[1].SetActive(true);
        m_damageWaves[3].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        m_damageWaves[1].SetActive(false);
        m_damageWaves[3].SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !m_complete)
        {
            m_inZone = true;
            m_speakerPanel.SetActive(true);
            StopCoroutines();
            StartCoroutine(EnterTalk());
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player") && !m_complete)
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
        m_dialogue[4].SetActive(false);
        m_dialogue[0].SetActive(true);
        m_dialogue[1].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        m_dialogue[0].SetActive(false);
        m_dialogue[1].SetActive(true);
    }
    protected IEnumerator LeaveTalk()
    {
        m_dialogue[1].SetActive(false); // Just incase any other text is blocking the screen from any other interactions
        m_dialogue[3].SetActive(false);
        m_dialogue[4].SetActive(false);
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
        m_dialogue[0].SetActive(true);
        m_dialogue[3].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        m_dialogue[0].SetActive(false);
        m_dialogue[3].SetActive(false);
    }

    protected void StopCoroutines()
    {
        StopCoroutine(EnterTalk());
        StopCoroutine(LeaveTalk());
        StopCoroutine(CompleteTalk());
    }

}
