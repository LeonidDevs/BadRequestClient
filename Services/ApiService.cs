using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using VittaClient.Models;

namespace VittaClient.Services
{


    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7083/api/");
        }

        public async Task<UserDTO> AuthAndGetUserIdAsynс(string username)
        {
            var response = await _httpClient.GetAsync($"Auth/{username}");

            if (response.IsSuccessStatusCode)
            {
                var userData = await response.Content.ReadFromJsonAsync<UserDTO>();
                return userData;
            }
            return null;
        }
        public async Task<List<OrderDTO>> GetOrdersAsync(UserDTO user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("orders", user);

                if (response.IsSuccessStatusCode)
                {
                    var orders = await response.Content.ReadFromJsonAsync<List<OrderDTO>>();
                    return orders ?? new List<OrderDTO>();
                }
                else
                {
                    MessageBox.Show($"Ошибка запроса{response.ReasonPhrase}");
                    return new List<OrderDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка запроса{ex.Message}");
                return new List<OrderDTO>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запроса{ex.Message}");
                return new List<OrderDTO>();
            }
        }

        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<List<ProductDTO>>("products");
                return products ?? new List<ProductDTO>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка запроса: {ex.Message}");
                return new List<ProductDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return new List<ProductDTO>();
            }
        }

        public async Task<HttpResponseMessage> CreateOrderAsync(OrderDTO order)
        {
            return await _httpClient.PostAsJsonAsync("orders/create", order);
        }

    }
}
