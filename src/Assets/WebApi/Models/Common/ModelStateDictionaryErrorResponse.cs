using System.Collections.Generic;

namespace Assets.WebApi.Models.Common
{
    public class ModelStateDictionaryErrorResponse
    {
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
