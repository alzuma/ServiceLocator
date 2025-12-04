namespace ServiceLocator.TestApi.Services.Singleton;

public interface ISingletonService
{
    void AddValue(string value);
    List<string> GetValues();
}
