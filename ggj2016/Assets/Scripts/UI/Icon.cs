using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Icon : MonoBehaviour
{
	[Header("Properties")]
	public soIcon _properties;

	[Space(15)]
	[Range(0, 1)]public float _progress;

	[Header("Components")]
	private Canvas _canvas;
	private Image _image;
	private RectTransform _rectTransform;

	public void Initialize()
	{
		// Initialize references
		_canvas = GetComponent<Canvas>();
		_image = GetComponentInChildren<Image>();
		_rectTransform = _image.GetComponent<RectTransform>();
	}

	public void ShowSprite(Sprite sprite)
	{
		Reset();

		_canvas.enabled = true;
		_image.sprite = sprite;
	}

	public void UpdateProgress(float progress)
	{
		_progress = progress;
		_image.fillAmount = _progress;
	}

	public void Reset()
	{
		_progress = 0;
		_rectTransform.localScale = Vector3.one;
		_canvas.enabled = false;
	}

	// Visual feedback
	public void Win()
	{
		UpdateProgress(1);

		StartCoroutine(R_Win());
	}

	IEnumerator R_Win()
	{
		float timer = _properties._scaleDuration;
		float scaleUp = 1 + _properties._scaleUp;
		float scaleCurrent = 1;
		
		while(timer > 0)
		{
			scaleCurrent = Mathf.Lerp(scaleUp, 1, timer / _properties._scaleDuration);
			_rectTransform.localScale = new Vector3(scaleCurrent, scaleCurrent, scaleCurrent);

			timer -= Time.deltaTime;

			yield return null;
		}

		Reset();
	}

	public void Fail()
	{
		StartCoroutine(R_Fail());
	}

	IEnumerator R_Fail()
	{
		for(int i = 0; i < _properties._flashes; i++)
		{
			yield return R_Flicker(_properties._flashLength);

			yield return new WaitForSeconds(_properties._flashLength);
		}

		Reset();
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
