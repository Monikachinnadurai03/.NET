using System;
using System.Collections.Generic;

namespace IMS_WebApplication.Models;

public partial class TransactionType
{
    public int TransactionTypeId { get; set; }

    public string TransactionType1 { get; set; } = null!;

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
}
