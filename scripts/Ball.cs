using Godot;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;

public partial class Ball : Area2D
{
	private const float DEFAULT_SPEED = 1000.0f;
	private const float BOUNCE_FACTOR_Y = 0.25f;

	private double _speed = DEFAULT_SPEED;
	private Vector2 _size;

	public Vector2 _direction = Vector2.Right;

	public bool IsLaunched { get; set; } = false;

	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ColorRect rect = GetNode("ColorRect") as ColorRect;
		_size = rect.Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!IsLaunched || GameManager.IsPaused)
		{
			return;
		}

		_speed += delta * 2;
		Vector2 v = new Vector2((float)(_direction.X * _speed * delta), (float)(_direction.Y * _speed * delta));
		this.Position += v;
	}

	public void Reset(Paddle paddle)
	{
		IsLaunched = false;

		_direction = paddle.GetPlayer() == GameManager.Player.A ? Vector2.Right : Vector2.Left;

		this.Position = paddle.GetFrontCenterPosition();

		_speed = DEFAULT_SPEED;
	}

	public void MoveY(float y)
	{
		Vector2 position = this.Position;
		this.Position = new Vector2(position.X, y);
	}

	public Vector2 GetSize()
	{
		return _size;
	}

	public void HitPaddle(Paddle paddle)
	{
		// +ve value: bottom of paddle
		// -ve value: top of paddle 
		double diffY = this.Position.Y - paddle.Position.Y;

		_direction = new Vector2((float)paddle.Width * 0.5f * (paddle.GetPlayer() == GameManager.Player.A ? 1 : -1), (float)diffY * BOUNCE_FACTOR_Y).Normalized();
	}

	public void HitWall()
	{
		_direction = new Vector2(_direction.X, _direction.Y * -1f);
	}
}



