using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxStayApi.Models;
[Table("Place")]

public class Place
{
    [Key, ForeignKey("Home")]
    public string place_id { get; set; }

    public string place_name { get; set; }

    public string image { get; set; }

    public int total_home { get; set; }

    // public List<Home> homes { get; set; }
    // public List<Tour> tours { get; set; }

}
