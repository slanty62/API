
namespace Microsoft.OpenApi
{
    internal class Models
    {
        internal class OpenApiInfo : OpenApi.OpenApiInfo
        {
            public string Title { get; set; }
            public string Version { get; set; }
            public string Description { get; set; }
            public object Contact { get; set; }
        }

        internal class OpenApiContact
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}