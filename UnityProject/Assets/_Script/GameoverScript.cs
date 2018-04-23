using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.PlayerManagement;
using States;
using UnityEngine;
using UnityEngine.SceneManagement;
using DefaultNamespace;

public class GameoverScript : MonoBehaviour {

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

		bool shouldGoToLobby = false;

		var validInputIndexes = PlayerInputUtils.GetValidInputIndexes();
		foreach (var index in validInputIndexes)
		{
			if (PlayerInputUtils.GetButtonDown(PlayerInput._A, index)) {
				shouldGoToLobby = true;
			}
		} 

		if (Input.GetKeyDown(KeyCode.Space) || shouldGoToLobby)
		{
			srenderer.sprite = highlightedSprite;
			audio.Play();

			Debug.Log("restart");

			GameManager.Instance.ResetScore();

			FSM.Instance.GotoState<LobbyState>();
			gameObject.AddComponent<PlayersManager>();

			//Load next scene here
		}
	}
}
