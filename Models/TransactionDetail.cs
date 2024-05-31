using System;
using System.Collections.Generic;

namespace IMS_WebApplication.Models;

public partial class TransactionDetail
{
    public int TransactionId { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public int? TransactionTypeId { get; set; }

    public decimal? Amount { get; set; }

    public virtual TransactionType? TransactionType { get; set; }
}
