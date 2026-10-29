using System.Collections;
using TMPro;
using UnityEngine;

public class NarrativeTrigger : MonoBehaviour
{
    [SerializeField] private string[] m_texts;
    [SerializeField] private int m_textnums;
    [SerializeField] private GameObject m_canvas;
    [SerializeField] private TMP_Text m_tmp;
    [SerializeField] private GameObject m_collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    IEnumerator Display_Text()
    {
        for (int i = 0; i < m_textnums; i++)
        {
            m_tmp.SetText(m_texts[i]);
            yield return new WaitForSeconds(3.0f);
        }
    }


}
