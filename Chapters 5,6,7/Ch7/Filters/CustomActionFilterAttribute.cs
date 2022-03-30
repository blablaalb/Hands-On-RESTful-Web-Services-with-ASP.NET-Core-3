using Microsoft.AspNetCore.Mvc;

namespace Ch7.Filters
{
    public class CustomActionFilterAttribute: TypeFilterAttribute
    {
        public CustomActionFilterAttribute(): base(typeof(CustomActionFilterAttribute))
        {

        }
    }
}
