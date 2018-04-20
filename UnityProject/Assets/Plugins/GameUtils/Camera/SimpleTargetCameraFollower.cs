using UnityEngine;
using System.Collections;

public class SimpleTargetCameraFollower : MonoBehaviour
{

  [SerializeField] private Transform player;

	// Use this for initialization
	void Start () {
	  if (player)
	  {
	    transform.position = new Vector3(player.position.x + 1, player.position.y,-10);
	  }
	}
	
	// Update is called once per frame
	void Update () {
	  if (player)
	  {
	    transform.position = new Vector3(player.position.x + 1, player.position.y,-10);
	  }
	}
}
