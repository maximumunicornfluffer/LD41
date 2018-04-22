using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.PlayerManagement;

public class TitleScript : MonoBehaviour {

	public Sprite highlightedSprite;
	private PlayerInput _input;
	private SpriteRenderer srenderer;
	private AudioSource audio;
	// Use this for initialization
	void Start () {

		srenderer = gameObject.GetComponent<SpriteRenderer>();
		audio = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (/*A button || */Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("ok!!!");
			srenderer.sprite = highlightedSprite;
			audio.Play();
			//Load next scene here
		}
	}
}
