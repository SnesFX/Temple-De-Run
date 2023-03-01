using UnityEngine;

public class GameCamera : MovingObject
{
	public MovingObject MainCameraFocusPoint;

	public CommonPlayer Player;

	private CharacterPlayer GamePlayer;

	public Transform Water;

	public float GroundHeight;

	public float FollowHeight;

	public float FollowDistance;

	public float FocusHeight;

	public float FollowHeightRun;

	public float FollowHeightJump;

	public float FollowHeightSlide;

	public float FollowDistanceRun;

	public float FollowDistanceJump;

	public float FollowDistanceSlide;

	public float FocusHeightRun;

	public float FocusHeightJump;

	public float FocusHeightSlide;

	public bool NeedsToDuckCamera;

	public float IntroDuration = 8f;

	public Vector3 StartLocation = new Vector3(90f, 110f, 150f);

	public Vector3 EndLocation = new Vector3(0f, 10f, 86f);

	public Vector3 TargetCameraLocation;

	public bool IsCameraShaking;

	public bool ShakeAfterDelay;

	public float ShakeDelay;

	public float ShakeMagnitudeAfterDelay;

	public float ShakeDurationAfterDelay;

	public float ShakeFrequencyMultiplierAfterDelay;

	public float TimeSinceCameraShakeStart;

	public float CameraShakeDamperRate;

	public float CameraShakeMagnitude;

	public float CameraShakeDuration;

	public float CameraShakeFrequencyMultiplier;

	public static GameCamera SharedInstance;

	public MainMenu MainMenuGUI;

	private bool ShowMainMenuAfterFollow;

	public bool SnapCamera;

	private void Awake()
	{
		SharedInstance = this;
		GamePlayer = Player as CharacterPlayer;
		base.transform.position = StartLocation;
		base.transform.LookAt(MainCameraFocusPoint.transform.position);
	}

	private void Start()
	{
		Reset();
		base.transform.position = StartLocation;
		base.transform.LookAt(MainCameraFocusPoint.transform.position);
	}

	public override void Update()
	{
		base.Update();
		if (ShowMainMenuAfterFollow)
		{
			if (!IsMoving())
			{
				ShowMainMenuAfterFollow = false;
				MainMenuGUI.Show();
			}
			else if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
			{
				SetMainMenuAniatmion(true);
			}
		}
		bool flag = false;
		bool flag2 = false;
		Vector3 vector;
		if (GamePlayer != null)
		{
			flag = GamePlayer.IsDead;
			flag2 = GamePlayer.IsJumping;
			vector = GamePlayer.Velocity;
		}
		else
		{
			vector = Player.GetComponent<PsuedoRunner>().CalculatedVelocityVector;
		}
		if (!flag && !GameController.SharedInstance.IsResurrecting)
		{
			MainCameraFocusPoint.transform.position = Player.transform.position;
			MainCameraFocusPoint.transform.rotation = Player.transform.rotation;
			if (NeedsToDuckCamera)
			{
				FollowHeight = FollowHeightSlide;
				FollowDistance = FollowDistanceSlide;
				FocusHeight = FocusHeightSlide;
			}
			else if (flag2)
			{
				FollowHeight = FollowHeightJump + (Player.transform.position.y - GroundHeight) * 0.5f;
				FollowDistance = FollowDistanceJump + (Player.transform.position.y - GroundHeight) * 0.5f;
				FocusHeight = FocusHeightJump + (Player.transform.position.y - GroundHeight) * 0.25f;
			}
			else
			{
				FollowHeight = FollowHeightRun;
				FollowDistance = FollowDistanceRun;
				FocusHeight = FocusHeightRun;
			}
			float num = ((!(GamePlayer != null)) ? Player.transform.position.y : GamePlayer.GroundHeight);
			float num2 = num - GroundHeight;
			if (num2 < 0f)
			{
				GroundHeight -= 50f * Time.smoothDeltaTime;
				if (GroundHeight < num)
				{
					GroundHeight = num;
				}
			}
			else if (num2 > 0f)
			{
				GroundHeight += 50f * Time.smoothDeltaTime;
				if (GroundHeight > num)
				{
					GroundHeight = num;
				}
			}
			MainCameraFocusPoint.transform.position = new Vector3(MainCameraFocusPoint.transform.position.x, GroundHeight + FocusHeight, MainCameraFocusPoint.transform.position.z);
			if (GameController.SharedInstance.IsIntroScene)
			{
				base.transform.LookAt(MainCameraFocusPoint.transform.position);
			}
			else
			{
				TargetCameraLocation = MainCameraFocusPoint.transform.position;
				TargetCameraLocation.y = GroundHeight;
				TargetCameraLocation += -MainCameraFocusPoint.transform.forward * FollowDistance + MainCameraFocusPoint.transform.up * FollowHeight;
				Vector3 vector2 = TargetCameraLocation - base.transform.position;
				if (vector2.magnitude > 0f && !flag)
				{
					float magnitude = vector2.magnitude;
					float num3 = vector.magnitude * 1.25f * Time.smoothDeltaTime;
					if (num3 >= magnitude || SnapCamera)
					{
						base.transform.position = TargetCameraLocation;
					}
					else
					{
						vector2.Normalize();
						base.transform.position += vector2 * num3;
					}
				}
				base.transform.LookAt(MainCameraFocusPoint.transform.position);
			}
		}
		if (GameController.SharedInstance.IsResurrecting)
		{
			base.transform.LookAt(MainCameraFocusPoint.transform.position);
		}
		if (IsCameraShaking)
		{
			TimeSinceCameraShakeStart -= Time.smoothDeltaTime;
			CameraShakeMagnitude -= Time.smoothDeltaTime * CameraShakeDamperRate;
			if (CameraShakeMagnitude <= 0f || TimeSinceCameraShakeStart > CameraShakeDuration)
			{
				IsCameraShaking = false;
			}
			else
			{
				float x = Mathf.Sin(TimeSinceCameraShakeStart * 35f * CameraShakeFrequencyMultiplier) * CameraShakeMagnitude;
				float y = Mathf.Sin(TimeSinceCameraShakeStart * 50f * CameraShakeFrequencyMultiplier) * CameraShakeMagnitude;
				base.transform.Translate(x, y, 0f);
			}
		}
		if (GameController.SharedInstance.IsGameStarted)
		{
			UpdateWater();
		}
	}

	private void UpdateWater()
	{
		Vector3 position = base.transform.position;
		int num = 200;
		float x = (int)(position.x / (float)num) * num;
		float z = (int)(position.z / (float)num) * num;
		Water.position = new Vector3(x, Water.position.y, z);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(TargetCameraLocation, 1f);
		Gizmos.DrawLine(base.transform.position, TargetCameraLocation);
	}

	public void Reset()
	{
		ClearFollowingPath();
		ShowMainMenuAfterFollow = false;
		IsCameraShaking = false;
		ShakeAfterDelay = false;
		TimeSinceCameraShakeStart = 0f;
		CameraShakeDamperRate = 0f;
		CameraShakeMagnitude = 0f;
		CameraShakeDuration = 0f;
		CameraShakeFrequencyMultiplier = 1f;
		FollowHeight = 31f;
		FollowDistance = 40f;
		FocusHeight = 14f;
		MainCameraFocusPoint.transform.position = new Vector3(Player.transform.position.x, FocusHeightRun, Player.transform.position.y);
		if (GameController.SharedInstance.IsIntroScene)
		{
			MainCameraFocusPoint.transform.rotation = Player.transform.rotation;
			base.transform.position = EndLocation;
			base.transform.LookAt(MainCameraFocusPoint.transform.position);
		}
		else
		{
			MainCameraFocusPoint.transform.rotation = Player.transform.rotation;
			base.transform.position = MainCameraFocusPoint.transform.position + -MainCameraFocusPoint.transform.forward * FollowDistanceRun + MainCameraFocusPoint.transform.up * FollowHeight;
			base.transform.LookAt(MainCameraFocusPoint.transform.position);
		}
	}

	public void SnapStartGame()
	{
		ShowMainMenuAfterFollow = false;
		StopFollowingPath();
		ClearFollowingPath();
		base.transform.position = EndLocation;
		GroundHeight = GamePlayer.GroundHeight;
		MainCameraFocusPoint.transform.position = new Vector3(Player.transform.position.x, FocusHeightRun, Player.transform.position.y);
	}

	public void SetMainMenuAniatmion(bool SnapToEnd = false)
	{
		if (!SnapToEnd)
		{
			base.transform.position = StartLocation;
			ClearFollowingPath();
			MovingObjectPathNode movingObjectPathNode = new MovingObjectPathNode();
			movingObjectPathNode.TimeStamp = 0f;
			movingObjectPathNode.Location = base.transform.position;
			movingObjectPathNode.Ease = true;
			AddFollowingPathNode(movingObjectPathNode);
			movingObjectPathNode = new MovingObjectPathNode();
			movingObjectPathNode.TimeStamp = IntroDuration;
			movingObjectPathNode.Location = EndLocation;
			movingObjectPathNode.Ease = true;
			AddFollowingPathNode(movingObjectPathNode);
			StartFollowingPath();
			ShowMainMenuAfterFollow = true;
		}
		else
		{
			SnapStartGame();
			MainMenuGUI.FadeIn(0f, 0.25f);
		}
	}

	public void Shake(float magnitude, float duration, float freqMult, float delay = 0f)
	{
		if (!(duration > 0f) || !(magnitude > 0f))
		{
			return;
		}
		if (delay > 0f)
		{
			ShakeMagnitudeAfterDelay = magnitude;
			ShakeDurationAfterDelay = duration;
			ShakeFrequencyMultiplierAfterDelay = freqMult;
			ShakeAfterDelay = true;
			ShakeDelay = delay;
			return;
		}
		if (!IsCameraShaking)
		{
			IsCameraShaking = true;
		}
		ShakeAfterDelay = false;
		TimeSinceCameraShakeStart = 0f;
		CameraShakeDamperRate = magnitude / duration;
		CameraShakeMagnitude = magnitude;
		CameraShakeDuration = duration;
		CameraShakeFrequencyMultiplier = freqMult;
	}
}
