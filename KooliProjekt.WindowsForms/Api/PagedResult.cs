using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.WindowsForms
{
    [ExcludeFromCodeCoverage]
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}