
using System.ComponentModel.DataAnnotations;

namespace LuxStayApi.Models.ViewModels;

public class UserViewModel
{   
    public string email { get; set; }
    public string phone { get; set; }
    public string name { get; set; }
    public string password { get; set; }
    public bool gender { get; set; } = true;
    public string address { get; set; } = "";
    public string role { get; set; } = "ROLE_USER";
    public bool verify { get; set; } = true;
}

