using UniRx;

public interface IUIPresenter
{
    //Get game difficulty
    ReactiveProperty<int> GetDifficulty();

    //Add score value
    void AddScore(int value);
}
