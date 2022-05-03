using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace FOPS.Blazor;

public static class Extends
{
    public static ValueTask<string> GetFssUrl(this ILocalStorageService localStorageService)
    {
        try
        {
            return localStorageService.GetItemAsStringAsync("FssServer");
        }
        catch
        {
            return ValueTask.FromResult("");
        }
    }
}