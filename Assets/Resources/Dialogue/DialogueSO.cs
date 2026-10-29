using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public int Length;
    public string[] Dialogue;
    public string[] OtherSpeakers;
    public int[] SpeakerOrder;
    public int[] Wait;
    public bool LevelEnd;
    public string NextScene;

}
