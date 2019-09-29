﻿using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    [SerializeField] Arrow[] arrows = null;
    [SerializeField] float nextDelay = 0.3f;

    public static GameManager Instance { get; private set; }
    public static Direction CurrentDirection { get; private set; }
    public static int PlayerScore { get; private set; }
    public static Arrow SelectedArrow { get; private set; }

    public static event System.Action<BeforeNextArrowEventArgs> BeforeNextArrow;
    public static event System.Action<OnMoveSuccessEventArgs> OnMoveSuccess;

    Direction inputDirection;
    Direction desiredDirection;
    Direction displayedDirection;
    float countdown;
    bool doInputCheck;
    int currentMoveIndex;

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion
    }

    void Update() {
        if (!AnimationManager.IsAnimating) {
            countdown -= Time.deltaTime;
            if (countdown <= 0f) {
                // Check if we're still handling input. If so, it means
                // no input was received so the player didn't do anything
                if (doInputCheck) {
                    var eventArgs = new BeforeNextArrowEventArgs(false);
                    PlayerScore -= SelectedArrow.ScoreValue;
                    BeforeNextArrow?.Invoke(eventArgs);
                    ResetValues();
                } else {
                    NextArrow();
                    doInputCheck = true;
                }
                return;
            }

            if (doInputCheck) {
                HandleInput();
            }
        }
    }

    void NextArrow() {
        // Hide the previous arrow and reset it
        if (SelectedArrow != null) {
            SelectedArrow.IsActive = false;
            SelectedArrow.ResetTransform();
        }

        // Randomly select an arrow based randomly on the weights
        SelectedArrow = arrows[SelectRandomWeightedIndex()];

        // Randomly choose a direction and set display direction based on the arrow modifier
        desiredDirection = DirectionUtility.GetRandomDirection();
        displayedDirection = (Direction)(((int)desiredDirection +
            SelectedArrow.DisplayedDirectionModifier) % DirectionUtility.kDirectionCount);

        SelectedArrow.Orientation = displayedDirection;
        SelectedArrow.IsActive = true;
        countdown = SelectedArrow.Duration;
    }

    // While or for loop ? make a choice. Algorithm (to be improved by
    // using random function only once) from the website
    // https://forum.unity.com/threads/random-numbers-with-a-weighted-chance.442190/
    int SelectRandomWeightedIndex() {
        int weightSum = arrows.Sum(a => a.Weight);
        for (int i = 0; i < arrows.Length - 1; i++) {
            if (Random.Range(0, weightSum) < arrows[i].Weight) {
                return i;
            }
            weightSum -= arrows[i].Weight;
        }
        return arrows.Length - 1;
    }

    void HandleInput() {
        if (Input.touchCount == 0 || !InputManager.GetInput(ref inputDirection)) {
            return;
        }

        // Check if the move list has been entirely iterated through
        if (currentMoveIndex < SelectedArrow.MoveCount) {
            int move = SelectedArrow.GetMove(currentMoveIndex);
            if ((int)inputDirection == (move + (int)displayedDirection) %
                    DirectionUtility.kDirectionCount) {
                // The input matches the move
                var eventArgs = new OnMoveSuccessEventArgs(currentMoveIndex);
                OnMoveSuccess?.Invoke(eventArgs);
                currentMoveIndex++;
            } else {
                PlayerScore -= (int)(SelectedArrow.ScoreValue * 1.5f);
                var eventArgs = new BeforeNextArrowEventArgs(false);
                BeforeNextArrow?.Invoke(eventArgs);
                ResetValues();
            }
        } else {
            // The arrow has been oriented successfully (no moves are left)
            bool isSuccess = inputDirection == desiredDirection;
            PlayerScore += isSuccess ? SelectedArrow.ScoreValue :
                -(int)(SelectedArrow.ScoreValue * 1.5f);
            var beforeNextArrowEventArgs = new BeforeNextArrowEventArgs(isSuccess);
            BeforeNextArrow?.Invoke(beforeNextArrowEventArgs);
            ResetValues();
        }
    }

    void ResetValues() {
        countdown = nextDelay;
        currentMoveIndex = 0;
        doInputCheck = false;
    }
}
