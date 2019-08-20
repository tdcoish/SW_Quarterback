using UnityEngine;

public static class UT_Extensions
{
    public static Vector3 Hermite(this Vector3 origin, Vector3 final, float factor) {
        return new Vector3(
            origin.x.Hermite(final.x, factor),
            origin.y.Hermite(final.y, factor),
            origin.z.Hermite(final.z, factor)
        );
    }

    public static float Hermite(this float from, float to, float percent) {
        float hermite = (3 * Mathf.Pow(percent, 2)) - (2 * Mathf.Pow(percent, 3));
        return Mathf.Lerp(from, to, hermite);
    }

    public static float Clamp180(this float angle) {
        if (angle > 180.0f) {
            float diff = angle % 180.0f;
            return -180 + diff;
        } else if (angle < -180.0f) {
            float diff = angle % 180.0f;
            return 180.0f - diff;
        } else {
            return angle;
        }
    }

    public static bool FloatIsEqual(this float value, float comparingValue) {
        float epsilon = 0.5f;
        return value >= comparingValue - epsilon && value <= comparingValue + epsilon;
    }

    public static float Normalize(this float value, float minValue, float maxValue) {
        return (value - minValue) / (maxValue - minValue);
    }

    public static float Denormalize(this float value, float minValue, float maxValue, bool inverted = false) {
        if (inverted == false) {
            return (value * (maxValue - minValue)) + minValue;
        } else {
            return maxValue - ((value * (maxValue - minValue)) + minValue);
        }
    }

    public static Color GetNormalizedColor(this float value, Color minColor, Color maxColor, float alpha) {
        float minH, minS, minV, maxH, maxS, maxV;
		Color.RGBToHSV(minColor, out minH, out minS, out minV);
		Color.RGBToHSV(maxColor, out maxH, out maxS, out maxV);
		
		float hue = Mathf.Lerp(maxH, minH, value);
		float saturation = Mathf.Lerp(maxS, minS, value);
		float newValue = Mathf.Lerp(maxV, minV, value);
		
		
		Color normalizedColor = Color.HSVToRGB(hue, saturation, newValue);
		normalizedColor.a = alpha;
		return normalizedColor;
    }
}