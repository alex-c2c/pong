using Godot;
using System;
using System.Diagnostics;

public partial class Paddle : Area2D
{
	[Export]
	private Ball _ball;

	[Export]
	private GameManager.Player _player;

	private const int _moveSpeed = 1000;
	private int _ballDir;
	private string _upAction;
	private string _downAction;
	private float _halfHeight;
	private Vector2 _initialPosition;

	public bool AutoPlay { get; set; } = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string name = Name.ToString().ToLower();
		_upAction = name + "_move_up";
		_downAction = name + "_move_down";
		_ballDir = name == "PaddleA" ? 1 : -1;

		_initialPosition = this.Position;

		Control panel = GetNode("Panel") as Control;
		_halfHeight = panel.Size.Y * 0.5f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.IsPaused)
		{
			return;
		}

		float input = 0;

		if (AutoPlay)
		{
			if (new Random().Next(0,101) > 50)
			{
				input = CheckBallWithinPaddleRange();
			}
		}
		else
		{
			input = Input.GetActionStrength(_downAction) - Input.GetActionStrength(_upAction);
		}

		Vector2 position = this.Position;
		position += new Vector2(0, (float)(input * _moveSpeed * delta));
		position.Y = Mathf.Clamp(position.Y, _halfHeight, GetViewportRect().Size.Y - _halfHeight);
		this.Position = position;

		if (!_ball.IsLaunched && GameManager.StartingPlayer == _player)
		{
			_ball.MoveY(position.Y);
		}
	}

	public Vector2 GetFrontCenterPosition()
	{
		Vector2 position = this.Position;

		if (_player == GameManager.Player.A)
		{
			return new Vector2(position.X + 30, position.Y);
		}
		else
		{
			return new Vector2(position.X - 30, position.Y);
		}
	}

	public GameManager.Player GetPlayer()
	{
		return _player;
	}

	public void ResetPosition()
	{
		this.Position = _initialPosition;
	}

	/// <summary>
	/// Checks position of ball against the height of paddle
	/// </summary>
	/// <returns>
	/// 		-1 if ball is higher than top edge of paddle, 
	/// 		0 if ball is within height of paddle, 
	/// 		1 if ball is lower than bottom edge of paddle.
	/// </returns>
	private int CheckBallWithinPaddleRange()
	{
		int result = 0;

		if (_ball.Position.Y > this.Position.Y + _halfHeight)
		{
			result = 1;
		}
		else if (_ball.Position.Y < this.Position.Y - _halfHeight)
		{
			result = -1;
		}

		return result;
	}
}
