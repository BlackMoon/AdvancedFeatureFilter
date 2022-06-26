namespace Library.Strategy
{
    public interface IRuleLoader
    {
        Task LoadRules(string path, char separator = ',');
    }
}

