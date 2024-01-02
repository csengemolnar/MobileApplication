using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookie.Model.DTO
{
    public class TastySearchResult
    {
        public string? name { get; set; }
        public string? yields { get; set; } //Servings 
        public int? total_time_minutes { get; set; }
        public string? thumbnail_url { get; set; }
        public List<TastyInstruction>? instructions { get;set;}

        public List<TastySection>? sections { get; set; }
    }
}
