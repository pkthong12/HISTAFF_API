namespace API.All.Services
{
    public interface ILanguageService
    {
        Task <string?> Translate(string key, string lang);
    }
}
