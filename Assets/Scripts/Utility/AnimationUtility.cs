using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class AnimationUtility {

    /// <summary>
    /// Creates a coroutine that plays an animation.
    /// </summary>
    /// <param name="speed">
    /// The animation speed.
    /// </param>
    /// <param name="curve">
    /// The curve to play the animation from.
    /// </param>
    //public static IEnumerator DoAnimation(float speed, AnimationCurve curve) {
    //    for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
    //        //backButtonImg.color = Color.Lerp(startColor, endColor, backBtnFadeAnimation.Evaluate(time)); //delegates ? but because of speed, just totally useless ?
    //        yield return null;
    //    }
    //}

    /// <summary>
    /// Creates a coroutine that moves a transform to a position.
    /// </summary>
    /// <param name="toMove">
    /// The object's transform to move.
    /// </param>
    /// <param name="destination">
    /// The position to move the transform to.
    /// </param>
    /// <param name="movement">
    /// The curve that describes the movement.
    /// </param>
    /// <param name="speed">
    /// How fast the movement is.
    /// </param>
    public static IEnumerator MoveToPosition(Transform toMove, Vector2 destination, AnimationCurve curve, float speed) {
        Vector2 startPosition = toMove.localPosition;

        for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
            toMove.localPosition = Vector2.LerpUnclamped(startPosition, destination, curve.Evaluate(time));

            yield return null;
        }
        toMove.localPosition = destination;
    }

    /// <summary>
    /// Creates a coroutine that moves a transform to a position.
    /// </summary>
    /// <param name="toMove">
    /// The object's transform to move.
    /// </param>
    /// <param name="destination">
    /// The position to move the transform to.
    /// </param>
    /// <param name="movement">
    /// The curve that describes the movement.
    /// </param>
    /// <param name="speed">
    /// How fast the movement is.
    /// </param>
    public static IEnumerator MoveToPosition(Transform toMove, Vector3 destination, AnimationCurve curve, float speed) {
        Vector3 startPosition = toMove.localPosition;

        for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
            toMove.localPosition = Vector3.LerpUnclamped(startPosition, destination, curve.Evaluate(time));

            yield return null;
        }
        toMove.localPosition = destination;
    }

    /// <summary>
    /// Creates a coroutine that moves a rect transform to a position with its anchored position.
    /// </summary>
    /// <param name="toMove">
    /// The object's rectTransform to move.
    /// </param>
    /// <param name="destination">
    /// The position to move the transform to.
    /// </param>
    /// <param name="movement">
    /// The curve that describes the movement.
    /// </param>
    /// <param name="speed">
    /// How fast the movement is.
    /// </param>
    public static IEnumerator MoveToPosition(RectTransform toMove, Vector2 destination, AnimationCurve curve, float speed) {
        Vector2 startPosition = toMove.anchoredPosition;

        for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
            toMove.anchoredPosition = Vector2.LerpUnclamped(startPosition, destination, curve.Evaluate(time));
            yield return null;
        }
        toMove.anchoredPosition = destination;
    }

    /// <summary>
    /// Creates a coroutine that fades in or out an image.
    /// </summary>
    /// <param name="image">
    /// The image to fade.
    /// </param>
    /// <param name="curve">
    /// The curve that describes the fading.
    /// </param>
    /// <param name="speed">
    /// How fast the fading is.
    /// </param>
    public static IEnumerator FadeImage(Image image, AnimationCurve curve, float speed) {
        Color color = image.color;

        for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
            image.color = new Color(color.r, color.g, color.b, curve.Evaluate(time));

            yield return null;
        }
    }

    /// <summary>
    /// Creates a coroutine that interpolates an image's color towards a target color.
    /// </summary>
    /// <param name="graphic">
    /// The image that holds the color to interpolate.
    /// </param>
    /// <param name="targetColor">
    /// The color to interpolate to.
    /// </param>
    /// <param name="curve">
    /// The curve that describes the color interpolation.
    /// </param>
    /// <param name="speed">
    /// How fast the interpolation is.
    /// </param>
    public static IEnumerator LerpColor(Graphic graphic, Color targetColor, AnimationCurve curve, float speed, float cycleOffset = 0f) {
        Color startColor = graphic.color;

        for (float time = cycleOffset; time < 1f; time += speed * Time.deltaTime) {
            graphic.color = Color.Lerp(startColor, targetColor, curve.Evaluate(time));

            yield return null;
        }
    }

    /// <summary>
    /// Creates a coroutine that interpolates a transform's scale towards a target scale.
    /// </summary>
    /// <param name="transform">
    /// The transform to scale.
    /// </param>
    /// <param name="targetScale">
    /// The scale to interpolate to.
    /// </param>
    /// <param name="curve">
    /// The curve that describes the scaling.
    /// </param>
    /// <param name="speed">
    /// How fast the interpolation is.
    /// </param>
    public static IEnumerator ScaleTransform(Transform transform, Vector3 targetScale, AnimationCurve curve, float speed) {
        Vector3 startScale = transform.localScale;

        for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
            transform.localScale = Vector3.Lerp(startScale, targetScale, curve.Evaluate(time));

            yield return null;
        }
    }

    public static IEnumerator RotateTransform(Transform transform, Vector3 targetRotation, AnimationCurve curve, float speed) {
        //Quaternion startRotation = transform.localRotation;

        Vector3 previousTargetRotation = new Vector3();
        Vector3 currentTargetRotation = new Vector3();
        for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
            //transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, curve.Evaluate(time))
            currentTargetRotation = Vector3.Lerp(Vector3.zero, targetRotation, curve.Evaluate(time));

            transform.Rotate(currentTargetRotation - previousTargetRotation);

            previousTargetRotation = currentTargetRotation;

            yield return null;
        }
    }
}
