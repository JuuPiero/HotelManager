
using System.ComponentModel.DataAnnotations;

namespace LuxStayApi.Models.ViewModels;

public class LoginRequest
{   
    
    [Required(ErrorMessage = "Email không được để trống")]
    // [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string email { get; set; }

    // public string phone { get; set; }

    // public string name { get; set; }

    public string password { get; set; }

    // public bool gender { get; set; }

    // public string address { get; set; }

    // public string role { get; set; }

    // public bool verify { get; set; }
}

