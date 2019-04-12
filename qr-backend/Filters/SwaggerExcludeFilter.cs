using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace qr_backend.Filters
{
    public class SwaggerExcludeFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var definition in swaggerDoc.Definitions)
            {
                foreach (var prop in definition.Value.Properties.ToList())
                {
                    if (prop.Value.MaxLength == int.MaxValue)
                    {
                        definition.Value.Properties.Remove(prop);
                    }
                }
            }
        }
    }
}
