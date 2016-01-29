using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Icon : MonoBehaviour
{
	[Header("Properties")]
	[Range(0, 1)]public float _progress;

	[Header("Components")]
	private Image _image;

	// Use this for initialization
	//void Start ()
	//{
	//	Initialize();
	//}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Space))
			StartCoroutine(R_Fail());
	}

	void Initialize(Sprite sprite)
	{
		// Initialize references
		_image = GetComponentInChildren<Image>();

		// Set properties
		_image.sprite = sprite;
	}

	void UpdateProgress(float progress)
	{
		_progress = progress;
		_image.fillAmount = _progress;
	}

	// Visual feedback
	public void Succes()
	{

	}

	IEnumerator R_Fail()
	{
		yield return R_Flicker(.1f);

		yield return new WaitForSeconds(.1f);

		yield return R_Flicker(.1f);
	}

	IEnumerator R_Flicker(float duration)
	{
		// Hide the image
		_image.enabled = false;

		yield return new WaitForSeconds(duration);

		// Show the image again
		_image.enabled = true;
	}
}
