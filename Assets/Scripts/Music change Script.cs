using UnityEngine;

public class MusicchangeScript : MonoBehaviour
{
    [SerializeField] private bool m_musicChange;
    [SerializeField] private Music m_musicScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_musicScript.ChangeMusic(m_musicChange);
    }

}
