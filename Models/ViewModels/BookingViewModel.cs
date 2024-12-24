
using System.ComponentModel.DataAnnotations;

namespace LuxStayApi.Models.ViewModels;

public class BookingViewModel
{   
    public int user_id { get; set; }

    public int home_id { get; set; }

    public DateTime date_check_in { get; set; }

    public DateTime date_check_out { get; set; }

    // public int total_price { get; set; }
}

