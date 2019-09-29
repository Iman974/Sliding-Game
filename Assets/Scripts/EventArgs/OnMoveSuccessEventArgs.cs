public class OnMoveSuccessEventArgs : IEventArgs {

    public int MoveIndex { get; set; }

    public OnMoveSuccessEventArgs(int moveIndex) {
        MoveIndex = moveIndex;
    }
}
