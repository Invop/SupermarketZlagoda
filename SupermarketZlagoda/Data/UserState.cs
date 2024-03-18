namespace SupermarketZlagoda.Data;

public class UserState
{
    private bool _isManager = true;

    public bool IsManager
    {
        get => _isManager;
        set => _isManager = value;
    }
}