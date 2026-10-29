using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Music : MonoBehaviour
{
    #region
    [SerializeField] private float m_health; // (vignette or written value)
    [SerializeField] private float m_resistance; // affects how much damage you take from attacks and music poison
    private float m_batteryMax = 150;
    [SerializeField] private float m_battery;
    [SerializeField] private float m_batteryEffects;
    [SerializeField] private Image m_batteryFill;
    [SerializeField] private bool m_battle;
    [SerializeField] private bool m_musicPlaying = true;
    [SerializeField] private bool m_safeZone;

    [SerializeField] private AudioSource m_corruptedMusic;
    [SerializeField] private AudioSource m_neutralMusic;
    [SerializeField] private AudioSource m_battleMusic;

    [SerializeField] private GameObject m_deathScreen;
    [SerializeField] private string m_currentScene; // To reload current scene on death
    #endregion

    void Awake()
    {
        m_battleMusic.enabled = false;
        m_neutralMusic.enabled = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //m_battery = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_musicPlaying && m_battery > 0)
        {
            //Drain battery with delta time. Make variable for mods, extra modes or upgrades
            m_battery -= Time.deltaTime * m_batteryEffects;
            m_batteryFill.fillAmount = (m_batteryMax - m_battery) / m_batteryMax;   // shows how much health they've lost
            m_neutralMusic.volume = m_battery / m_batteryMax;
            m_battleMusic.volume = m_battery / m_batteryMax;
            if (m_battery < (m_batteryMax * 0.2))
            {
                m_corruptedMusic.enabled = true;
                m_corruptedMusic.volume = ((m_batteryMax * 0.2f) - m_battery) * 5 / m_batteryMax;
            }
            if (m_battery <= 0)
            {
                m_battery = 0;
                m_musicPlaying = false;
            }
        }
        else if ((m_battery == 0 || m_musicPlaying == false) && m_safeZone == false)
        {
            m_health -= Time.deltaTime * m_resistance;
        }
        else if (!m_musicPlaying)
        {
            m_corruptedMusic.volume = 1;
        }
        if (m_health <= 0)
        {
            StartCoroutine(DeathScreen());
        }
    }

    public void Set_SafeZone(bool safe)
    {
        m_safeZone = safe;
    }

    public void Music_ONOFF()
    {
        if (m_musicPlaying)
        {
            m_musicPlaying = false; // Stop battery drain
            m_neutralMusic.mute = true;
            m_battleMusic.mute = true;
            m_corruptedMusic.mute = false;
            // Attacks now deal damage instead of draining battery
            // If leave "safe area", drain health (vignette)
        }
        else
        {
            if (m_battery > 0)
            {
                m_musicPlaying = true; // Stop battery drain (maybe increase later will an upgrade)
                m_neutralMusic.mute = false;
                m_battleMusic.mute = false;
                m_corruptedMusic.mute = true;
                // Attacks deal damage instead of draining battery
                // If leave "safe area", drain health (vignette)
            }
        }
    }
    public void ReloadMusic()
    {
        // music sent back to start
        // Attacks deal damage instead of draining battery
        // If leave "safe area", drain health (vignette)
    }
    public void GetBattery()
    {
        if (m_battery < 20 && (m_battery + (m_batteryMax * 0.2f)) > 20)
        {
            m_corruptedMusic.enabled = false;
        }
        m_battery += m_batteryMax * 0.2f;
        if (m_battery > m_batteryMax)
        {
            m_battery = m_batteryMax;
        }
        m_batteryFill.fillAmount = (m_batteryMax - m_battery) / m_batteryMax;   //changes even when walkman is inactive
    }
    public void Hit(float damage)
    {
        if (m_battery > 0)
        {
            m_battery -= damage * m_batteryEffects;
            m_batteryFill.fillAmount = (m_batteryMax - m_battery) / m_batteryMax;
        }
        else
        {
            m_health -= damage * m_resistance;
        }
    }
    public void ChangeMusic(bool val)
    {
        // Change music to a battle theme if detected
        if (val == true)
        {
            m_neutralMusic.enabled = false;
            m_battleMusic.enabled = true;
        }
        else
        {
            m_battleMusic.enabled = false;
            m_neutralMusic.enabled = true;
        }
        // Ideas for later:
        // Dont change back until all enemies in that cluster have been defeated 
        // continue playing for 5 seconds after last enemy detection.
    }

    private IEnumerator DeathScreen()
    {
        m_deathScreen.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(m_currentScene);
    }
}
