using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class AnimationUtility {

    public enum FadeType {
        In,
        Out
    }

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
    /// Creates a coroutine that moves the given transform to a position using lerp with the given curve.
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
    /// How fast the movement will be.
    /// </param>
    public static IEnumerator MoveToPosition(Transform toMove, Vector2 destination, AnimationCurve movement, float speed = 1f) {
        Vector3 startPosition = toMove.position;

        for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
            Vector2 result = Vector2.LerpUnclamped(startPosition, destination, movement.Evaluate(time));
            toMove.position = new Vector3(result.x, result.y, startPosition.z);

            yield return null;
        }
        toMove.position = new Vector3(destination.x, destination.y, startPosition.z);
    }

    /// <summary>
    /// Creates a coroutine that moves the given rectTransform's anchored position to a position using lerp with the given curve.
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
    /// How fast the movement will be.
    /// </param>
    public static IEnumerator MoveToPosition(RectTransform toMove, Vector2 destination, AnimationCurve movement, float speed = 1f) {
        Vector2 startPosition = toMove.anchoredPosition;

        for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
            toMove.anchoredPosition = Vector2.LerpUnclamped(startPosition, destination, movement.Evaluate(time));
            yield return null;
        }
        toMove.anchoredPosition = destination;
    }

    public static IEnumerator FadeImage(Image image, FadeType fadeType, float speed, float startModifier = 0f) {
        float alphaEnd = fadeType == FadeType.In ? 1f : 0f;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f - alphaEnd + startModifier);

        while (!Mathf.Approximately(image.color.a, alphaEnd)) {
            Color clr = image.color;
            image.color = new Color(clr.r, clr.g, clr.b, Mathf.Lerp(clr.a, alphaEnd, speed));

            yield return null;
        }
    }
}
