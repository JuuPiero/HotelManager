
using System.ComponentModel.DataAnnotations;

namespace LuxStayApi.Models.ViewModels;

public class SignupRequest
{   
    
    [Required(ErrorMessage = "Email không được để trống")]
    // [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string email { get; set; }

    [Required(ErrorMessage = "Phone không được để trống")]
    public string phone { get; set; }

    public string name { get; set; }

    [Required(ErrorMessage = "Pass không được để trống")]
    public string password { get; set; }

    // public bool gender { get; set; }
    public string address { get; set; } = "";
}

