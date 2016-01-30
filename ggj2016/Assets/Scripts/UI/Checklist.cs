using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checklist : MonoBehaviour
{
	[Header("Properties")]
	public Vector3 _startPosition;
	public Vector2 _listDirection;

	public List<TaskDefinition> _toDoList = new List<TaskDefinition>();
	public List<TaskDefinition> _doneList = new List<TaskDefinition>();

	// Redraw the checklists
	void UpdateChecklists()
	{


	}
}
