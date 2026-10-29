using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class Glassguy : EnemyScript
{

    public PlayableDirector m_playableD;
    [SerializeField] private GameObject m_activator;
    private bool m_hasPlayed = false;

    //public void Play()
    //{
    //    m_playableD.Play();
    //}

    private void OnTriggerEnter(Collider collider)
    {
        if (!m_hasPlayed && collider.CompareTag("Player"))
        {
            m_playableD.Play(); //play timeline
            m_hasPlayed = true; //prevents repeating animation
            //StartCoroutine(Changecam());// change camera to track enemy

            if (collider.tag == "Player")
            {
                m_enemy.Follow = true;
            }
            else if (collider.tag == "Light Attack")
            {
                m_enemy.Health -= 5;
            }
            else if (collider.tag == "Heavy Attack")
            {
                m_enemy.Health -= 10;
            }
            if (m_enemy.Health < -0)
            {
                Destroy(this);
            }
        }
    }

    //private IEnumerator Changecam()
    //{
    //    m_cam.CameraLockOn(m_transform);
    //    yield return new WaitForSeconds(7.5f);
    //    m_cam.Return2Player();
    //}
}