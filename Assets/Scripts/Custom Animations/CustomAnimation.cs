using UnityEngine;
using System.Collections;
using System;

public abstract class CustomAnimation : ScriptableObject {

    [SerializeField] protected AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] protected float animationSpeed = 1f;

    //[SerializeField] private float delay;
    //public float Delay {
    //    get { return delay; }
    //}

    /// <summary>
    /// The type of the animated component.
    /// </summary>
    public abstract Type AnimatedComponent { get; }

    /// <summary>
    /// Creates the animation that this object represents.
    /// </summary>
    /// <param name="component">
    /// The component that is animated.
    /// </param>
    /// <param name="direction">
    /// The current slide direction.
    /// </param>
    public abstract IEnumerator GetAnimation(Component component);
}
