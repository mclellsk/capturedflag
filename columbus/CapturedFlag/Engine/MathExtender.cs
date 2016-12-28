using UnityEngine;

namespace CapturedFlag.Engine
{
	public static class MathExtender
	{
        /// <summary>
        /// Considers two floats equal if the difference between the two is less than the threshold.
        /// </summary>
        /// <param name="float1">First float.</param>
        /// <param name="float2">Second float.</param>
        /// <param name="threshold">Threshold of difference between floats.</param>
        /// <returns>Equality of two floats.</returns>
        public static bool IsEqual(this float float1, float float2, float threshold)
        {
            return (Mathf.Abs(float1 - float2) <= threshold);
        }

        /// <summary>
        /// Rounds a float based on each level of precision raised by a factor of 10.
        /// </summary>
        /// <param name="value">Float to round.</param>
        /// <param name="precision">Precision factor.</param>
        /// <returns>Rounded float.</returns>
        public static float Round (float value, int precision)
		{
			var b = Mathf.Pow (10f, precision);
			return Mathf.Round (value * b) / b;
		}

        /// <summary>
        /// Compares two Vector3 values and determines if they are equal based on the threshold limit.
        /// </summary>
        /// <param name="v1">First vector.</param>
        /// <param name="v2">Second vector.</param>
        /// <param name="threshold">Maximum difference between two axis values in a vector.</param>
        /// <returns></returns>
        public static bool IsVectorEqual (Vector3 v1, Vector3 v2, float threshold)
        {
            if (!IsEqual(v1.x, v2.x, threshold))
            {
                return false;
            }

            if (!IsEqual(v1.y, v2.y, threshold))
            {
                return false;
            }

            if (!IsEqual(v1.z, v2.z, threshold))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compares two Vector3 values and determines if they are equal based on the level of precision.
        /// </summary>
        /// <param name="v1">First vector.</param>
        /// <param name="v2">Second vector.</param>
        /// <param name="precision">Precision factor.</param>
        /// <returns>Equality of two vectors.</returns>
		public static bool IsVectorEqual (Vector3 v1, Vector3 v2, int precision = 1)
		{
			if (Mathf.Abs(Round (v1.x, precision) - Round (v2.x, precision)) > 0)
			{
				return false;
			}
			
			if (Mathf.Abs(Round (v1.y, precision) - Round (v2.y, precision)) > 0)
			{
				return false;
			}

			if (Mathf.Abs(Round (v1.z, precision) - Round (v2.z, precision)) > 0)
			{
				return false;
			}

			return true;
		}

        /// <summary>
        /// Similar to modulo, however it does not return negative values when a is negative. Negative-free modulo.
        /// </summary>
        /// <param name="a">First integer</param>
        /// <param name="b">Second integer</param>
        /// <returns>Result of a mod b.</returns>
        public static int nfmod(int a, int b)
        {
            return a - b * Mathf.FloorToInt(((float)a / b));
        }
    }
}

