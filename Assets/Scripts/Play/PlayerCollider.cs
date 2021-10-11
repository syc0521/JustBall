using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
	public GameObject player;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("ball"))
		{
			player.GetComponent<Player>().BallCollide();
		}
	}
}
