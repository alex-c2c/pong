using Godot;
using System;
using System.Diagnostics;

public partial class Main : Node2D
{
	[Export]
	private Ball _ball;

	[Export]
	private Paddle _paddleA;

	[Export]
	private Paddle _paddleB;

	[Export]
	private Label _labelScoreA;

	[Export]
	private Label _labelScoreB;

	[Export]
	private Node2D _menu;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//GameManager.PreloadAudio();

		GameManager.StartingPlayer = GameManager.Player.A;

		GameManager.IsPaused = false;

		_menu.Visible = false;

		_ball?.Reset(_paddleA);

		UpdateScoreLabel();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_text_clear_carets_and_selection"))
		{
			AudioManager.PlaySFX("cancel", this);

			_menu.Visible = !GameManager.IsPaused;
			GameManager.IsPaused = _menu.Visible;
		}
		else if (Input.IsActionJustPressed("ui_accept"))
		{
			if (!_ball.IsLaunched)
			{
				_ball.IsLaunched = true;
				
				AudioManager.PlaySFX("select", this);
			}
		}
	}

	private void _on_ball_area_entered(Area2D area)
	{
		string areaName = area.Name.ToString().ToLower();

		if (areaName.Contains("goala"))
		{
			AudioManager.PlaySFX("beep", this);

			PlayerScored(GameManager.Player.B);
		}
		else if (areaName.Contains("goalb"))
		{
			AudioManager.PlaySFX("beep", this);

			PlayerScored(GameManager.Player.A);
		}
		else if (areaName.Contains("paddle"))
		{
			AudioManager.PlaySFX("select", this);

			_ball?.HitPaddle(areaName == "paddlea" ? _paddleA : _paddleB);
		}
		else if (areaName.Contains("wall"))
		{
			AudioManager.PlaySFX("select", this);

			_ball?.ReverseDirectionY();
		}
	}

	private void _on_button_reset_game_pressed()
	{
		AudioManager.PlaySFX("confirm", this);

		GameManager.IsPaused = false;
		_menu.Visible = false;

		GameManager.ScoreA = 0;
		GameManager.ScoreB = 0;

		UpdateScoreLabel();

		_paddleA.ResetPosition();
		_paddleB.ResetPosition();

		GameManager.StartingPlayer = GameManager.Player.A;
		_ball.Reset(_paddleA);
	}


	private void _on_button_quit_game_pressed()
	{
		AudioManager.PlaySFX("confirm", this);

		GetTree().Quit();
	}


	private void _on_checkbox_auto_player_a_toggled(bool button_pressed)
	{
		if (button_pressed)
		{
			AudioManager.PlaySFX("confirm", this);
		}
		else
		{
			AudioManager.PlaySFX("cancel", this);
		}

		_paddleA.AutoPlay = button_pressed;
	}


	private void _on_checkbox_auto_player_b_toggled(bool button_pressed)
	{
		if (button_pressed)
		{
			AudioManager.PlaySFX("confirm", this);
		}
		else
		{
			AudioManager.PlaySFX("cancel", this);
		}

		_paddleB.AutoPlay = button_pressed;
	}

	private void PlayerScored(GameManager.Player player)
	{
		if (player == GameManager.Player.A)
		{
			_ball?.Reset(_paddleB);
			GameManager.ScoreA += 1;
			GameManager.StartingPlayer = GameManager.Player.B;
		}
		else
		{
			_ball?.Reset(_paddleA);
			GameManager.ScoreB += 1;
			GameManager.StartingPlayer = GameManager.Player.A;
		}

		UpdateScoreLabel();
	}

	private void UpdateScoreLabel()
	{
		_labelScoreA.Text = GameManager.ScoreA.ToString();
		_labelScoreB.Text = GameManager.ScoreB.ToString();
	}
}
