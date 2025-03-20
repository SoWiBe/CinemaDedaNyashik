using System.Net;
using System.Text;
using System.Text.Json;
using FirstMy.Bot.Models;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Exceptions;

namespace FirstMy.Bot.Services.Core;

public abstract class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    protected ApiService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = configuration.GetSection("CinemaApi").Get<ApiSettings>()!.Url;
    }
    
    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        return await SendRequestAsync<T>(endpoint, HttpMethod.Get);
    }

    protected async Task<T?> PostAsync<T>(string endpoint, object? data)
    {
        return await SendRequestAsync<T>(endpoint, HttpMethod.Post, data);
    }
    
    protected async Task<bool> PostAsync(string endpoint, object? data)
    {
        return await SendRequestAsync(endpoint, HttpMethod.Post, data);
    }

    protected async Task<T?> PutAsync<T>(string endpoint, object? data)
    {
        return await SendRequestAsync<T>(endpoint, HttpMethod.Put, data);
    }

    protected async Task<T?> DeleteAsync<T>(string endpoint)
    {
        return await SendRequestAsync<T>(endpoint, HttpMethod.Delete);
    }

    private async Task<T?> SendRequestAsync<T>(string endpoint, HttpMethod httpMethod, object? data = null)
    {
        try
        {
            var request = $"{_baseUrl}{endpoint}";
            var requestMessage = new HttpRequestMessage(httpMethod, request);

            if (data is not null)
            {
                var jsonContent = JsonSerializer.Serialize(data);
                requestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(requestMessage);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return default;

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(responseContent);
            if (result is null)
                throw new ApiRequestException(string.Format("Error after calling method {Method} with {Uri}", httpMethod,
                    request));
            
            return result;
        }
        catch (HttpRequestException  ex)
        {
            throw new ApiRequestException($"Ошибка при выполнении запроса: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new ApiRequestException($"Ошибка при десериализации ответа: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new ApiRequestException($"Произошла ошибка: {ex.Message}", ex);
        }
    }
    
    private async Task<bool> SendRequestAsync(string endpoint, HttpMethod httpMethod, object? data = null)
    {
        try
        {
            var request = $"{_baseUrl}{endpoint}";
            var requestMessage = new HttpRequestMessage(httpMethod, request);

            if (data is not null)
            {
                var jsonContent = JsonSerializer.Serialize(data);
                requestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(requestMessage);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return false;

            return true;
        }
        catch (HttpRequestException  ex)
        {
            throw new ApiRequestException($"Ошибка при выполнении запроса: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new ApiRequestException($"Ошибка при десериализации ответа: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new ApiRequestException($"Произошла ошибка: {ex.Message}", ex);
        }
    }
}