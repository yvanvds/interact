using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace InteractClient.Droid.Sensors
{
	public class StepCounter
	{
		private const int ACCEL_RING_SIZE = 50;
		private const int VEL_RING_SIZE = 10;
		private const float STEP_THRESHOLD = 4.0f;
		private const int STEP_DELAY_NS = 250000000;

		private int accelRingCounter = 0;
		private float[] accelRingX = new float[ACCEL_RING_SIZE];
		private float[] accelRingY = new float[ACCEL_RING_SIZE];
		private float[] accelRingZ = new float[ACCEL_RING_SIZE];
		private int velRingCounter = 0;
		private float[] velRing = new float[VEL_RING_SIZE];
		private long lastStepTimeNs = 0;
		private float oldVelocityEstimate = 0;

		public bool IsNewStep(long timeNS, float[] values)
		{
			accelRingCounter++;
			accelRingX[accelRingCounter % ACCEL_RING_SIZE] = values[0];
			accelRingY[accelRingCounter % ACCEL_RING_SIZE] = values[1];
			accelRingZ[accelRingCounter % ACCEL_RING_SIZE] = values[2];

			float[] worldZ = new float[3];
			worldZ[0] = sum(accelRingX) / Math.Min(accelRingCounter, ACCEL_RING_SIZE);
			worldZ[1] = sum(accelRingY) / Math.Min(accelRingCounter, ACCEL_RING_SIZE);
			worldZ[2] = sum(accelRingZ) / Math.Min(accelRingCounter, ACCEL_RING_SIZE);

			float normalization_factor = norm(worldZ);
			worldZ[0] = worldZ[0] / normalization_factor;
			worldZ[1] = worldZ[1] / normalization_factor;
			worldZ[2] = worldZ[2] / normalization_factor;

			// Next step is to figure out the component of the current acceleration
			// in the direction of world_z and subtract gravity's contribution

			float currentZ = dot(worldZ, values) - normalization_factor;
			velRingCounter++;
			velRing[velRingCounter % VEL_RING_SIZE] = currentZ;

			float velocityEstimate = sum(velRing);

			bool result = false;
			if (velocityEstimate > STEP_THRESHOLD && oldVelocityEstimate <= STEP_THRESHOLD
					&& (timeNS - lastStepTimeNs > STEP_DELAY_NS))
			{
				result = true;
				lastStepTimeNs = timeNS;
			}

			oldVelocityEstimate = velocityEstimate;
			return result;
		}

		private float sum(float[] array)
		{
			float result = 0;
			foreach(var value in array)
			{
				result += value;
			}
			return result;
		}

		private float[] cross(float[] arrayA, float[] arrayB)
		{
			float[] result = new float[3];
			result[0] = arrayA[1] * arrayB[2] - arrayA[2] * arrayB[1];
			result[1] = arrayA[2] * arrayB[0] - arrayA[0] * arrayB[2];
			result[2] = arrayA[0] * arrayB[1] - arrayA[1] * arrayB[0];
			return result;
		}

		private float norm(float[] array)
		{
			float result = 0;
			foreach(var value in array)
			{
				result += value * value;
			}
			return (float)Math.Sqrt(result);
		}

		private float dot(float[] a, float[] b)
		{
			return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
		}

		private float[] normalize(float[] a)
		{
			float[] result = new float[a.Length];
			float norm = this.norm(a);

			for (int i = 0; i < a.Length; i++)
			{
				result[i] = a[i] / norm;
			}

			return result;
		}
	}
}