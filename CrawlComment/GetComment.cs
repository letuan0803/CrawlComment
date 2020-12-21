using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CrawlComment.Model.CommentModal;
using static CrawlComment.Model.ProductModal;

namespace CrawlComment
{
    class GetComment
    {
        public GetComment()
        {

        }
        public void GetCmt(long itemid = 5051930544, int limit = 50, long shopid = 308378250)
        {
            using (HttpClient client = new HttpClient())
            {
                int numberLoop = 0;
                HttpResponseMessage result;
                int offset = 0;
                int totalcomment = 0;
                do
                {
                    UriBuilder uriBuilder = new UriBuilder("https://shopee.vn/api/v2/item/get_ratings");
                    uriBuilder.Query = $"filter=0&flag=1&itemid={itemid}&limit={limit}&offset={offset}&shopid={shopid}&type=0";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Accept.Clear();

                    result = client.GetAsync(uriBuilder.Uri).Result;
                    CommentResult commentResult = JsonConvert.DeserializeObject<CommentResult>(result.Content.ReadAsStringAsync().Result);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK && commentResult.data.item_rating_summary.rcount_with_context > 0)
                    {
                        totalcomment = commentResult.data.item_rating_summary.rcount_with_context;

                        MongoClient mgclient = new MongoClient("mongodb://localhost:27017");
                        IMongoDatabase database = mgclient.GetDatabase("commentInfo");
                        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("comment");

                        List<BsonDocument> listBsonDocument = new List<BsonDocument>();
                        foreach (var item in commentResult.data.ratings)
                        {
                            listBsonDocument.Add(item.ToBsonDocument());
                        }
                        collection.InsertManyAsync(listBsonDocument);
                    };
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        Thread.Sleep(10000);
                        continue;
                    }
                    numberLoop++;
                    offset += limit;
                    Thread.Sleep(1000);
                } while (numberLoop * limit < totalcomment);
            }

        }

        public void GetProduct(int categoryid = 2365, int limit = 50, int numberloop = 500, string by = "relevancy", string order = "desc", string pageType = "search")
        {
            // categoryid = 2365, là ngành hàng điện tử
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result;
                int offset = 0;
                for (int i = 0; i < numberloop; i++)
                {
                    UriBuilder uriBuilder = new UriBuilder("https://shopee.vn/api/v2/search_items/");
                    uriBuilder.Query = $"by={by}&limit={limit}&match_id={categoryid}&newest={offset}&order={order}&page_type={pageType}&version=2";

                    client.DefaultRequestHeaders.Add("accept", "*/*");
                    client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66");

                    result = client.GetAsync(uriBuilder.Uri).Result;
                    ProductResult productResult = JsonConvert.DeserializeObject<ProductResult>(result.Content.ReadAsStringAsync().Result);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK && productResult!=null)
                    {

                        MongoClient mgclient = new MongoClient("mongodb://localhost:27017");
                        IMongoDatabase database = mgclient.GetDatabase("commentInfo");
                        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("product");

                        List<BsonDocument> listBsonDocument = new List<BsonDocument>();
                        foreach (var item in productResult.items)
                        {
                            listBsonDocument.Add(item.ToBsonDocument());
                        }
                        collection.InsertManyAsync(listBsonDocument);
                    };
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        Thread.Sleep(10000);
                        continue;
                    }
                    offset += limit;
                    Thread.Sleep(1000);
                    Console.WriteLine(i);
                }
            }

        }
    }
}
