
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxStayApi.Models;
[Table("Booking")]
public class Booking 
{
    [Key]
    public int booking_id { get; set; }

    public int user_id { get; set; }

    public int home_id { get; set; }

    public DateTime date_check_in { get; set; }

    public DateTime date_check_out { get; set; }

    public int total_price { get; set; }
}
