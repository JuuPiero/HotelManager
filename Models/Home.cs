using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxStayApi.Models;
[Table("Home")]
public class Home
{

    [Key]
    public int home_id { get; set; }

    public string home_name { get; set; }

    public string home_type { get; set; }

    public int room_number { get; set; }

    public int price { get; set; }

    // public Place place { get; set; }
    public string place_id { get; set; }


    public string image_intro { get; set; }

    public string address { get; set; }

    public string short_description { get; set; }

    public string detail_description { get; set; }

    public bool? restore { get; set; } = true;

    // List<ImagesDetail> images_detail { get; set; }
}
