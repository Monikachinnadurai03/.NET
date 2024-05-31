using System;
using System.Collections.Generic;

namespace IMS_WebApplication.Models;

public partial class ClientInvestment
{
    public int ClientInvestmentId { get; set; }

    public int ClientId { get; set; }

    public int InvestmentId { get; set; }

    public decimal InvestmentAmount { get; set; }

    public int Quantity { get; set; }

    public DateOnly? InvestmentDate { get; set; }

    public int TransactionId { get; set; }

    public virtual required Client Client { get; set; }

    public virtual required Investment Investment { get; set; }
    
}
