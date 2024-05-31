using System;
using System.Collections.Generic;

namespace IMS_WebApplication.Models;

public partial class Investment
{
    public int InvestmentId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly PurchaseDate { get; set; }

    public decimal PurchasePrice { get; set; }

    public int InvestmentTypeId { get; set; }

    public virtual ICollection<ClientInvestment> ClientInvestments { get; set; } = new List<ClientInvestment>();

    public virtual InvestmentType InvestmentType { get; set; } = null!;
}
