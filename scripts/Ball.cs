using Godot;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;

public partial class Ball : Area2D
{
	private const float DEFAULT_SPEED = 1000.0f;
	private double _speed = DEFAULT_SPEED;

	public Vector2 _direction = Vector2.Right;

	public bool IsLaunched { get; set; } = false;

	private Vector2 _size;


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
		diffY *= 0.25f; // reduces the angle of reflection

		double factor = Math.Sqrt(Math.Pow(paddle.Width * 0.5f, 2f) + Math.Pow(diffY, 2f));

		float x = (float)(paddle.Width * 0.5f / factor);
		float y = (float)(diffY / factor);

		_direction = new Vector2(x * (paddle.GetPlayer() == GameManager.Player.A ? 1 : -1), y);

		//_direction = new Vector2((float)(paddle.Width * 0.5f / factor) *, (float)(diffY / factor));

		//int randomY = new Random().Next(-80, 81);

		//_direction = new Vector2(_direction.X > 0 ? -1 : 1, randomY / 100f);

		_speed = DEFAULT_SPEED;
	}

	public void ReverseDirectionY()
	{
		_direction = new Vector2(_direction.X, _direction.Y > 0 ? -1 : 1);
	}
}



