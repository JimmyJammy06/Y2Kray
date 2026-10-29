using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayer : MonoBehaviour
{

    public PlayableDirector m_playableD;
    [SerializeField] private Collider m_activator;
    //public void Play()
    //{
    //    m_playableD.Play();
    //}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Hit and played");
            m_playableD.Play(); //play timeline
            m_activator.enabled = false;   //prevents repeating animation
            //StartCoroutine(Changecam());// change camera to track enemy
        }
    }
}
