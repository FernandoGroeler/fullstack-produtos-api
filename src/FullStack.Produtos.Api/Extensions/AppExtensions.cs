using Asp.Versioning;

namespace FullStack.Produtos.Api;

internal static class AppExtensions
{
    internal static void MapEndpoints(this WebApplication app)
    {
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .HasApiVersion(new ApiVersion(3))
            .ReportApiVersions()
            .Build();
        
        var versionedGroup = app
            .MapGroup("api/v{apiVersion:apiVersion}")
            .WithApiVersionSet(apiVersionSet);        
        
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();        
        foreach (var endpoint in endpoints)
            endpoint.MapEndpoint(versionedGroup);
    }    
}