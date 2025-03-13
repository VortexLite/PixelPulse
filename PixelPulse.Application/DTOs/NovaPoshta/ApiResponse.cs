namespace PixelPulse.Application.DTOs.NovaPoshta;

public class ApiResponse<T>
{
    public bool success { get; set; }
    public List<T> data { get; set; }
}