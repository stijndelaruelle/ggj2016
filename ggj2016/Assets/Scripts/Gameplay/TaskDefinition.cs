using UnityEngine;
using System.Collections;

public enum TaskCategoryType
{
    Time = 0,
    Productivity = 1,
    Wardrobe = 2,
    Smell = 3,
    Entertainment = 4,
}

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

    [Space(15)]
    [Header("Score")]

    [SerializeField]
    private TaskCategoryType m_TaskCategory;
    public TaskCategoryType TaskCategory
    {
        get { return m_TaskCategory; }
    }

    [SerializeField]
    private int m_Weight;
    public int Weight
    {
        get { return m_Weight; }
    }
}