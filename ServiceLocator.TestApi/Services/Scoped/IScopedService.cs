namespace ServiceLocator.TestApi.Services.Scoped;

public interface IScopedService
{
    void AddValue(string value);
    List<string> GetValues();
}
