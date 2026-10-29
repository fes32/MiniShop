
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace MiniShop.Api.Configuration;

public static class ApiApplicationExtensions
{
    public static WebApplication UseMiniShopApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
