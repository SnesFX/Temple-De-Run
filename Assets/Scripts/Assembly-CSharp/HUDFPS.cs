using UnityEngine;

public class HUDFPS : MonoBehaviour
{
	private const int MAXSAMPLES = 10;

	public float updateInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeleft;

	private UILabel Label;

	private int tickindex;

	private float ticksum;

	private float[] ticklist = new float[10];

	private double CalcAverageTick(float newtick)
	{
		ticksum -= ticklist[tickindex];
		ticksum += newtick;
		ticklist[tickindex] = newtick;
		if (++tickindex == 10)
		{
			tickindex = 0;
		}
		return (double)ticksum / 10.0;
	}

	private void Start()
	{
		Label = GetComponent<UILabel>();
		timeleft = updateInterval;
	}

	private void Update()
	{
		timeleft -= Time.smoothDeltaTime;
		float num = Time.timeScale / Time.deltaTime;
		accum += num;
		frames++;
		double num2 = CalcAverageTick(num);
		if (FTPSGraph.Instance != null)
		{
			FTPSGraph.Instance.AddPoint(num);
		}
		if ((double)timeleft <= 0.0)
		{
			float num3 = accum / (float)frames;
			string text = string.Format("{0:F2} FPS", num3);
			Label.text = text;
			if (num3 < 30f)
			{
				Label.color = Color.yellow;
			}
			else if (num3 < 10f)
			{
				Label.color = Color.red;
			}
			else
			{
				Label.color = Color.green;
			}
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}
}
