namespace ServiceLocator.TestApi.Services.Transient;

public interface ITransientService
{
    void AddValue(string value);
    List<string> GetValues();
}
