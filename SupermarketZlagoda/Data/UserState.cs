namespace SupermarketZlagoda.Data;

public class UserState
{
    private bool _isManager = false;

    public bool IsManager
    {
        get => _isManager;
        set => _isManager = value;
    }
}