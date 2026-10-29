using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueScript : MonoBehaviour
{
    [SerializeField] private DialogueSO m_dialogue;
    [SerializeField] private TextMeshProUGUI m_talkText;
    [SerializeField] private TextMeshProUGUI m_speakerNameText;
    [SerializeField] private GameObject m_dialogueBackground;
    [SerializeField] private GameObject[] m_speakerBox;
    //[SerializeField] private string m_nextScene;

    void Start()
    {
        m_dialogueBackground.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(GoThroughDialogue());
    }

    private IEnumerator GoThroughDialogue()
    {
        for (int i = 0; i < m_dialogue.Length; i++)
        {
            if (m_dialogue.SpeakerOrder[i] == 0)
            {
                m_speakerBox[1].SetActive(false);
                m_speakerBox[0].SetActive(true);
            }
            else
            {
                m_speakerBox[1].SetActive(true);
                m_speakerBox[0].SetActive(false);
                if ((m_dialogue.OtherSpeakers).Length > 1)
                {
                    m_speakerNameText.SetText(m_dialogue.OtherSpeakers[m_dialogue.SpeakerOrder[i]-1]);
                }

            }
            m_talkText.SetText(m_dialogue.Dialogue[i]);
            yield return new WaitForSeconds(m_dialogue.Wait[i]);
        }
        if (m_dialogue.LevelEnd) //If this is dialogue at the end of the level
        {
            SceneManager.LoadScene(m_dialogue.NextScene);
        }
    }
}
