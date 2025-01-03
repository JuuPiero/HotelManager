using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxStayApi.Models;


public class TourViewModel
{
   
    public string tour_name { get; set; }

    public string tour_type { get; set; }

    public int tour_number { get; set; }

    public int price { get; set; }

    public string place_id { get; set; }

    public string image { get; set; }

    public string address { get; set; }

    public string short_description { get; set; }

    public string detail_description { get; set; }
}
