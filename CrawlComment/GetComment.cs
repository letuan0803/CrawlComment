using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CrawlComment
{
    class GetComment
    {
        public GetComment()
        {

        }
        public void GetCmt()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://shopee.vn/api/v2/item/get_ratings");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
    }
}
