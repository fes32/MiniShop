
namespace MiniShop.Web.Services;

public static class DatabasePath
{
    public static string GetDatabasePath()
    {
        var appFolder = AppContext.BaseDirectory;

        return Path.Combine(appFolder, "minishop.db");
    }
}
