namespace WiSave.Income.WebApi.Endpoints;

public static class Extensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapIncomeEndpoints();
        
        return app;
    }
}