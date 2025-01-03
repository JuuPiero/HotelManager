﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxStayApi.Models;
[Table("BookingTour")]
public class BookingTour
{
    [Key]
    public int booking_tour_id { get; set; }
    public int user_id { get; set; }
    public int tour_id { get; set; }
    public DateTime date_booking { get; set; }
    public decimal total_price { get; set; }
    public decimal? price { get; set; }
    public int people { get; set; }
}
