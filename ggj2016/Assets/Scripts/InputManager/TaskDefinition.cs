using UnityEngine;
using System.Collections;

[CreateAssetMenu (menuName = "GGJ16/Task Definition")]
public class TaskDefinition : ScriptableObject
{
    [SerializeField]
    private string m_Title;
    public string Title
    {
        get { return m_Title; }
    }

    [SerializeField]
    private string m_Description;
    public string Description
    {
        get { return m_Description; }
    }

    [SerializeField]
    private Sprite m_Sprite;
    public Sprite Sprite
    {
        get { return m_Sprite; }
    }

    [SerializeField]
    private float m_TimeToComplete;
    public float TimeToComplete
    {
        get { return m_TimeToComplete; }
    }
}