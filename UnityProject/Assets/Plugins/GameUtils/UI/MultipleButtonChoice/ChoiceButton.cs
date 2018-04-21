using System;
using Plugins.Utils.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceButton : /*EventTrigger*/ MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{

	[SerializeField]
	private Image _normalBg;
	[SerializeField]
	private Image _selectedBg;
	[SerializeField]
	private Text _text;

	private int _idx=-1;

	public Action<int> OnPointerEnterActon;
	public Action<int> OnPointerClickActon;

	public void Init(int idx, string name)
	{
		_idx = idx;
		_text.text = name;
	}

	public string Name
	{
		get { return _text.text; }
	}

	public void Select(bool value)
	{
		_normalBg.gameObject.SetActive(!value);
		_selectedBg.gameObject.SetActive(value);
	}

	public void Plop(bool value)
	{
		OnPointerEnter(null);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnPointerEnterActon?.Invoke(_idx);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnPointerClickActon?.Invoke(_idx);
	}
}
