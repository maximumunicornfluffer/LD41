using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.PlayerManagement;
using States;
using UnityEngine.SceneManagement;
using DefaultNamespace;

public class GameoverScript : MonoBehaviour
{

	public Sprite highlightedSprite;
	public Text scoreText;

	private PlayerInput _input;
	private SpriteRenderer srenderer;
	private AudioSource audio;

	private float timeToWaitBeforeUpdate = 2.0f;
	private float waitingTime = 0.0f;

	// Use this for initialization
	void Start ()
	{


		srenderer = gameObject.GetComponent<SpriteRenderer> ();
		srenderer.enabled = false;
		audio = gameObject.GetComponent<AudioSource> ();

		scoreText.text = "Your " + GameManager.Instance.GetScoreText ();
	}

	// Update is called once per frame
	void Update ()
	{

		waitingTime = waitingTime + Time.deltaTime;

		if (!srenderer.enabled && waitingTime > timeToWaitBeforeUpdate)
		{
			srenderer.enabled = true;
		}
			
		if (srenderer.enabled)
		{
			bool shouldGoToLobby = false;
			var validInputIndexes = PlayerInputUtils.GetValidInputIndexes ();
			foreach (var index in validInputIndexes)
			{
				if (PlayerInputUtils.GetButtonDown (PlayerInput._A, index))
				{
					shouldGoToLobby = true;
				}
			} 

			if (Input.GetKeyDown (KeyCode.Space) || shouldGoToLobby)
			{
				srenderer.sprite = highlightedSprite;
				audio.Play ();

				Debug.Log ("restart");

				GameManager.Instance.ResetScore ();

				FSM.Instance.GotoState<LobbyState> ();
				gameObject.AddComponent<PlayersManager> ();

				//Load next scene here
			}
		}
	}
}
