using UnityEngine;

public class BeforeNextArrowEventArgs : IEventArgs {

    public bool IsSuccess { get; set; }

    public BeforeNextArrowEventArgs(bool isSuccess) {
        IsSuccess = isSuccess;
    }
}
