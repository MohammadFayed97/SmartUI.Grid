namespace SmartUI.Grid.Services
{
    using Newtonsoft.Json;
    using System.Text.Json;
    using AntiRap.Core.Pagination;
    using System.Net.Http;
    using SmartUI.Grid.Models;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpFeatureService<TModel> : IHttpFeatureService<TModel>
         where TModel : class
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public HttpFeatureService(HttpClient httpClient)
        {
            _client = httpClient;
            //_client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<PagedResponse<TModel>> GetAsync(string url, FilterationData filterQuery = null)
        {
            //string fullUrl = url;
            //if(parameters is not null)
            //{
            //    fullUrl = $"{url}?pagesize={parameters.PageSize}&pagenumber={parameters.PageNumber}";
            //}

            var jsonParameters = JsonConvert.SerializeObject(filterQuery);
            //var body = new StringContent(jsonParameters, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Filter", jsonParameters);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception("Field To Load");

            var header = response.Headers.GetValues("X-Pagination").First();
            var PageResponse = new PagedResponse<TModel>
            {
                Items = System.Text.Json.JsonSerializer.Deserialize<List<TModel>>(content, _options),
                MetaData = System.Text.Json.JsonSerializer.Deserialize<MetaData>(header, _options),
            };
            return PageResponse;
        }
        public async Task<IEnumerable<TModel>> GetByPredicateAsync(string url, List<FilterRuleModel> queryFilterRules)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var index = 1;
            foreach (FilterRuleModel queryFilter in queryFilterRules)
            {
                var jsonParameters = JsonConvert.SerializeObject(queryFilter);
                request.Headers.Add($"X-Filter{index}", jsonParameters);
                index++;
            }

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception("Field To Load");

            var items = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<TModel>>(content, _options);
            //var header = response.Headers.GetValues("X-Pagination").First();
            //var PageResponse = new PagedResponse<TModel>
            //{
            //    MetaData = System.Text.Json.JsonSerializer.Deserialize<MetaData>(header, _options),
            //};
            return items;
        }

        public async Task<IEnumerable<TModel>> GetDataAsync(string url)
        {
            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Field To Load");

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<TModel>>(content);
        }
    }
}