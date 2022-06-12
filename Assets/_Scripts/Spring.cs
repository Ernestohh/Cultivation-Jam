using UnityEngine;
using System;

[System.Serializable]
public class Spring
{
	public float state;
	public float target_state;
	public float strength;
	public float vel;
	public float damping;

	public Spring(float state, float target_state, float strength, float damping)//spring classýnýn constructoru
	{
		this.state = state;
		this.target_state = target_state;
		this.strength = strength;
		this.damping = damping;
		this.vel = 0.0f;
	}
	public void Update()
	{
		bool linear_springs = false;
   //     if (this.damping==0.001f)
   //     {
			//linear_springs = true;
   //     }
		if (linear_springs)
		{
			this.state = Mathf.MoveTowards(this.state, this.target_state, this.strength * Time.deltaTime * 0.05f);
		}
		else
		{
			this.vel += (this.target_state - this.state) * this.strength * Time.deltaTime;
			this.vel *= Mathf.Pow(this.damping, Time.deltaTime);
			this.state += this.vel * Time.deltaTime;
		}

	}
}
public class RevImitation
{
	public float state;
	public float target_state;
	public float strength;
}