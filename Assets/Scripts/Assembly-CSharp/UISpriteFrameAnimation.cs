using UnityEngine;

public class UISpriteFrameAnimation : MonoBehaviour
{
	public string FrameNamePrefix;

	public string FrameNamePostfix;

	public int FirstFrame;

	public int Frames;

	public float FPS;

	public bool DisableAtEnd;

	public bool PingPong;

	public int OnFrame;

	private UISprite SpriteComponent;

	private float TimeTillNextFrame;

	private int Direction = 1;

	private void Start()
	{
		OnFrame = FirstFrame;
		SpriteComponent = GetComponent<UISprite>();
	}

	private void Update()
	{
		if (SpriteComponent == null || FPS <= 0f || !SpriteComponent.enabled)
		{
			return;
		}
		TimeTillNextFrame -= Time.smoothDeltaTime;
		if (!(TimeTillNextFrame <= 0f))
		{
			return;
		}
		TimeTillNextFrame += 60f / FPS / 60f;
		string spriteName = FrameNamePrefix + OnFrame + FrameNamePostfix;
		SpriteComponent.spriteName = spriteName;
		SpriteComponent.MakePixelPerfect();
		OnFrame += Direction;
		if (OnFrame == FirstFrame + Frames)
		{
			if (PingPong)
			{
				Direction = -1;
			}
			else
			{
				OnFrame = FirstFrame;
				if (DisableAtEnd)
				{
					SpriteComponent.enabled = false;
				}
			}
		}
		if (OnFrame == FirstFrame)
		{
			Direction = 1;
		}
	}
}
