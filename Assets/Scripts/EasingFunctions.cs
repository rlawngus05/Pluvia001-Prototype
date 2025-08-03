using System;
using UnityEngine;

public static class EasingFunctions
{
    //* Generic fallback ease
    public static float EaseIn(float t) => Mathf.Pow(2, 10 * (t - 1));
    public static float EaseOut(float t) => 1 - Mathf.Pow(2, -10 * t);
    public static float EaseInAndOut(float t)
    {
        if (t < 0.5f) return Mathf.Pow(2, 20 * t - 10) / 2;
        return (2 - Mathf.Pow(2, -20 * t + 10)) / 2;
    }

    //* Sine
    public static float EaseInSine(float t) => 1 - Mathf.Cos((t * Mathf.PI) / 2);
    public static float EaseOutSine(float t) => Mathf.Sin((t * Mathf.PI) / 2);
    public static float EaseInOutSine(float t) => -(Mathf.Cos(Mathf.PI * t) - 1) / 2;

    // Quad
    public static float EaseInQuad(float t) => t * t;
    public static float EaseOutQuad(float t) => 1 - (1 - t) * (1 - t);
    public static float EaseInOutQuad(float t) => t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;

    //!* Cubic
    public static float EaseInCubic(float t) => t * t * t;
    public static float EaseOutCubic(float t) => 1 - Mathf.Pow(1 - t, 3);
    public static float EaseInOutCubic(float t) => t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;

    //* Quart
    public static float EaseInQuart(float t) => t * t * t * t;
    public static float EaseOutQuart(float t) => 1 - Mathf.Pow(1 - t, 4);
    public static float EaseInOutQuart(float t) => t < 0.5 ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2;

    //* Quint
    public static float EaseInQuint(float t) => t * t * t * t * t;
    public static float EaseOutQuint(float t) => 1 - Mathf.Pow(1 - t, 5);
    public static float EaseInOutQuint(float t) => t < 0.5 ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;

    //* Expo
    public static float EaseInExpo(float t) => t == 0 ? 0 : Mathf.Pow(2, 10 * t - 10);
    public static float EaseOutExpo(float t) => t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
    public static float EaseInOutExpo(float t)
    {
        if (t == 0) return 0;
        if (t == 1) return 1;
        if (t < 0.5f) return Mathf.Pow(2, 20 * t - 10) / 2;
        return (2 - Mathf.Pow(2, -20 * t + 10)) / 2;
    }

    //* Circ
    public static float EaseInCirc(float t) => 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));
    public static float EaseOutCirc(float t) => Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
    public static float EaseInOutCirc(float t)
    {
        if (t < 0.5f) return (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2;
        return (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;
    }

    //* Back
    public static float EaseInBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;
        return c3 * t * t * t - c1 * t * t;
    }

    public static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;
        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }

    public static float EaseInOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        if (t < 0.5f)
            return (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2;
        return (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
    }

    //* Elastic
    public static float EaseInElastic(float t)
    {
        if (t == 0 || t == 1) return t;
        const float c4 = (2 * Mathf.PI) / 3;
        return -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * c4);
    }

    public static float EaseOutElastic(float t)
    {
        if (t == 0 || t == 1) return t;
        const float c4 = (2 * Mathf.PI) / 3;
        return Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
    }

    public static float EaseInOutElastic(float t)
    {
        const float c5 = (2 * Mathf.PI) / 4.5f;
        if (t == 0 || t == 1) return t;

        if (t < 0.5f)
            return -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2;
        return (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
    }

    //* Bounce
    public static float EaseInBounce(float t) => 1 - EaseOutBounce(1 - t);

    public static float EaseOutBounce(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1 / d1)
            return n1 * t * t;
        else if (t < 2 / d1)
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        else if (t < 2.5 / d1)
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        else
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
    }

    public static float EaseInOutBounce(float t)
    {
        if (t < 0.5f) return (1 - EaseOutBounce(1 - 2 * t)) / 2;
        return (1 + EaseOutBounce(2 * t - 1)) / 2;
    }

    //* Custom
    public static float FullyCubicIn(float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        float oneMinusT = 1f - t;
        return 3f * oneMinusT * oneMinusT * t + 3f * oneMinusT * t * t + t * t * t;
    }
}
