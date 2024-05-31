using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_WebApplication.Models;

public partial class Client
{
    
    [Required(ErrorMessage = "Client ID is required")]
    public int ClientId { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    //[RegularExpression(@"^[^\d]+$", ErrorMessage = "First Name cannot contain numbers")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name must contain only letters")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last Name is required")]
    //[RegularExpression(@"^[^\d]+$", ErrorMessage = "Last Name cannot contain numbers")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name must contain only letters")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Invalid Email Address. Must be a valid Gmail address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Phone Number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
    public string PhoneNumber { get; set; } = null!;

    //[Required(ErrorMessage = "City is required")]
    public string City { get; set; } = null!;

    //[Required(ErrorMessage = "State is required")]
    public string State { get; set; } = null!;

    [Required(ErrorMessage = "PAN CARD No is required")]
    [RegularExpression(@"[A-Z]{5}[0-9]{4}[A-Z]{1}", ErrorMessage = "PAN Card number must be in this format FIKJH5625I")]
    public string PanCardNo { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; }

    [Required(ErrorMessage = "Age is required")]
    [Range(18, 150, ErrorMessage = "Age must be between 18 and 150")]
    public int Age { get; set; }

    public virtual ICollection<ClientInvestment> ClientInvestments { get; set; } = new List<ClientInvestment>();
}
