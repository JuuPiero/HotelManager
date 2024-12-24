using System.ComponentModel.DataAnnotations;

namespace LuxStayApi.Models;
public class ImagesDetail
{
    [Key]

    public int image_id { get; set; }

    public int home_id { get; set; }

    public string image_url { get; set; }
}

