using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxStayApi.Models;

[Table("User")]
public class User
{
    [Key]
    public int user_id { get; set; }

    public string email { get; set; }

    public string phone { get; set; }

    public string name { get; set; }

    public string password { get; set; }

    public bool gender { get; set; } = true;

    public string address { get; set; } = "";

    public string role { get; set; }

    public bool verify { get; set; }

}

