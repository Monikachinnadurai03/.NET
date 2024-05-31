using System;
using System.Collections.Generic;

namespace IMS_WebApplication.Models;

public partial class InvestmentType
{
    public int InvestmentTypeId { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Investment> Investments { get; set; } = new List<Investment>();
}
