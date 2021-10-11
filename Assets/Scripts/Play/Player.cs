using DG.Tweening;
using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : IPlayer
{
	private Vector2 Direction;
	public float speed;
	private float horizontal, vertical;
	private float angle;
	private float speedInit;
	private bool isHit = false;
	public GameController gameController;
	private Animator animator;
	private AnimatorOverrideController controller;
	void Start()
	{
		speedInit = speed;
		animator = GetComponent<Animator>();
		controller = new AnimatorOverrideController(animator.runtimeAnimatorController);
		animator.runtimeAnimatorController = controller;
		SetAnimation();
	}

	void Update()
	{
		horizontal = Device.Direction.X;
		vertical = Device.Direction.Y;
		angle = Device.Direction.Angle;
		
		if (gameController.playerScore.playerHealth[PlayerIndex] <= 0)
		{
			GameController.players[PlayerIndex] = 0;
			GameController.playerCount--;
			gameObject.SetActive(false);
		}
	}
    private void FixedUpdate()
    {
        if (GameController.GameStart)
        {
			PlayerMove();
		}
	}

    private void PlayerMove()
	{
		if (Mathf.Abs(horizontal) >= 0.2f || Mathf.Abs(vertical) >= 0.2f)
		{
			if (Compare(angle, 0))
			{
				transform.localEulerAngles = new Vector3(0, 0, 0);
			}
			else if (Compare(angle, 45))
			{
				transform.localEulerAngles = new Vector3(0, 0, -45);
			}
			else if (Compare(angle, 90))
			{
				transform.localEulerAngles = new Vector3(0, 0, -90);
			}
			else if (Compare(angle, 135))
			{
				transform.localEulerAngles = new Vector3(0, 0, -135);
			}
			else if (Compare(angle, 180))
			{
				transform.localEulerAngles = new Vector3(0, 0, -180);
			}
			else if (Compare(angle, 225))
			{
				transform.localEulerAngles = new Vector3(0, 0, -225);
			}
			else if (Compare(angle, 270))
			{
				transform.localEulerAngles = new Vector3(0, 0, -270);
			}
			else if (Compare(angle, 315))
			{
				transform.localEulerAngles = new Vector3(0, 0, -315);
			}
			Direction = new Vector2(horizontal, vertical).normalized;
			transform.Translate(Direction * speed * Time.deltaTime, Space.World);
		}
		//speed = Device.Action3.LastState ? 2 * speedInit : speedInit;
	}

	private bool Compare(float a, float b)
	{
		return Mathf.Abs(a - b) < 35.0f;
	}

	public void BallCollide()
	{
		if (!isHit)
		{
			isHit = true;
			gameController.ShakeCamera();
			StartCoroutine(Blink());
			Vibrate(0.85f);
			Invoke(nameof(UnVibrate), 0.25f);
			gameController.playerScore.playerHealth[PlayerIndex]--;
		}
	}

	public void Vibrate(float intensity)
	{
		Device.Vibrate(intensity);
	}

	public void UnVibrate()
	{
		Device.StopVibration();
	}

	private IEnumerator Blink()
	{
		for (int i = 0; i < 7; i++)
		{
			var r = GetComponent<SpriteRenderer>().color.r;
			var g = GetComponent<SpriteRenderer>().color.g;
			var b = GetComponent<SpriteRenderer>().color.b;
			GetComponent<SpriteRenderer>().color = new Color(r, g, b, 0);
			yield return new WaitForSeconds(0.12f);
			GetComponent<SpriteRenderer>().color = new Color(r, g, b, 1);
			yield return new WaitForSeconds(0.12f);
		}
		isHit = false;
		yield break;
	}

    public void SetAnimation()
    {
		controller["Character1_main"] = gameController.animationAsset.animationClip[PlayerType];
	}
}
