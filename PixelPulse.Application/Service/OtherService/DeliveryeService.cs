using System.Text;
using Newtonsoft.Json;
using PixelPulse.Application.DTOs.NovaPoshta;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.Application.Service.OtherService;

public class DeliveryeService : IDeliveryeService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiToken = "7424f1382545dde014233e977ae8bafa";
    private readonly string _apiUrl = "https://api.novaposhta.ua/v2.0/json/";

    public DeliveryeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<List<City>> GetCitiesAsync()
    {
        var requestData = new
        {
            apiKey = _apiToken,
            modelName = "AddressGeneral",
            calledMethod = "getCities",
            methodProperties = new { Limit = "10000" }
        };

        var response = await SendRequestAsync<City>(requestData);
        return response?.data ?? new List<City>();
    }
    
    public async Task<List<City>> SearchCitiesAsync(string searchQuery)
    {
        var requestData = new
        {
            apiKey = _apiToken,
            modelName = "AddressGeneral",
            calledMethod = "getCities",
            methodProperties = new { FindByString = searchQuery, Limit = "10" }
        };

        var response = await SendRequestAsync<City>(requestData);
        return response?.data ?? new List<City>();
    }

    public async Task<List<Department>> GetDepartmentsAsync(string cityRef)
    {
        var requestData = new
        {
            apiKey = _apiToken,
            modelName = "Address",
            calledMethod = "getWarehouses",
            methodProperties = new { CityRef = cityRef }
        };

        var response = await SendRequestAsync<Department>(requestData);
        return response?.data ?? new List<Department>();
    }
    
    private async Task<ApiResponse<T>> SendRequestAsync<T>(object requestData)
    {
        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent);
        }

        return null;
    }
}