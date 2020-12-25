using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CrawlComment.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
        /// <summary>
        /// thực hiện lấy tất cả cmt của 1 sản phẩm
        /// </summary>
        /// <param name="itemid">id sản phẩm</param>
        /// <param name="limit">limit của 1 api</param>
        /// <param name="shopid">id của shop</param>
        public void GetCmt(long itemid = 5051930544, int limit = 50, long shopid = 308378250)
        {
            using (HttpClient client = new HttpClient())
            {
                int numberLoop = 0;
                HttpResponseMessage result;
                int offset = 0;
                int? totalcomment = 0;
                int errorLoop = 0;
                do
                {
                    UriBuilder uriBuilder = new UriBuilder("https://shopee.vn/api/v2/item/get_ratings");
                    uriBuilder.Query = $"filter=0&flag=1&itemid={itemid}&limit={limit}&offset={offset}&shopid={shopid}&type=0";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Accept.Clear();

                    result = client.GetAsync(uriBuilder.Uri).Result;
                    CommentResult commentResult = JsonConvert.DeserializeObject<CommentResult>(result.Content.ReadAsStringAsync().Result);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK && commentResult?.data?.item_rating_summary?.rcount_with_context > 0)
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
                        Console.WriteLine($"shopid: {shopid}, itemid: {itemid}, numberloop: {numberLoop}, totalcommentwwithcontext: {totalcomment}");
                    }
                    else
                    {
                        if (errorLoop > 5)
                        {
                            break;
                        }
                        Console.WriteLine($"error shopid: {shopid}, itemid: {itemid}, numberloop: {numberLoop}, totalcommentwwithcontext: {totalcomment}");
                        Thread.Sleep(10000);
                        errorLoop++;
                        continue;
                    }
                    numberLoop++;
                    offset += limit;
                    errorLoop = 0;
                    Thread.Sleep(1500);
                } while (numberLoop * limit < totalcomment);
            }

        }

        /// <summary>
        /// thực hiện lấy danh sách sản phẩm về
        /// </summary>
        /// <param name="categoryid">id danh mục sản phẩm</param>
        /// <param name="limit">số bản ghi mỗi lần lấy về</param>
        /// <param name="offset">số bản ghi bỏ qua</param>
        /// <param name="numberloop">số lần lặp</param>
        /// <param name="by">lọc theo kiểu nào, ở dây dùng relavation: những bình luận phổ biến nhất</param>
        /// <param name="order">sắp xếp theo kiểu nào</param>
        /// <param name="pageType">kiểu tìm kiếm</param>
        public void GetProduct(int categoryid = 2365, int limit = 50, int offset = 0, int numberloop = 500, string by = "relevancy", string order = "desc", string pageType = "search")
        {
            // categoryid = 2365, là ngành hàng điện tử
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result;
                ProductResult productResult;
                UriBuilder uriBuilder = new UriBuilder("https://shopee.vn/api/v2/search_items/");

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("accept", "*/*");
                //client.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9,vi;q=0.8");
                //client.DefaultRequestHeaders.Add("cookie", "_gcl_au=1.1.310041953.1608562782; csrftoken=ekyn5ZukHjEOdEOxSkiRphu7bcwzGkWJ; REC_T_ID=29640d1b-439d-11eb-acf8-3c15fb3af44b; SPC_EC=-; SPC_U=-; SPC_F=wAXheBGpNAVwszcXoK5IwngfW1VQD7Zg; welcomePkgShown=true; _hjid=5b116431-5f21-4352-aec9-24f5dad7962d; SPC_SI=mall.Wl7jnF0KPoe39UkWZoGr2tN1V0rmfC0A; _gid=GA1.2.180358208.1608735817; SPC_R_T_ID=\"mIsOEpdzfEVWeFWbCBi/vQ3PVXXIZfyhOPA+/X16RDKR9g5SyTrumUzj/W6B8JlCz3vm5suU7FN042FbolJTRHrgkouYh7xpgimuo9vm/WI=\"; SPC_T_IV=\"VP9z9tBSHJ4NpWpCUKhcNQ==\"; SPC_R_T_IV=\"VP9z9tBSHJ4NpWpCUKhcNQ==\"; SPC_T_ID=\"mIsOEpdzfEVWeFWbCBi/vQ3PVXXIZfyhOPA+/X16RDKR9g5SyTrumUzj/W6B8JlCz3vm5suU7FN042FbolJTRHrgkouYh7xpgimuo9vm/WI=\"; _ga_M32T05RVZT=GS1.1.1608742193.8.0.1608742193.0; AMP_TOKEN=%24NOT_FOUND; _ga=GA1.2.844213790.1608562793; cto_bundle=JP3SS194SjZETjlFeERLaEVTJTJGJTJGVlNpaWl6WHVuS3FmUTFBeGs4ZGUlMkZJSTRwU3hyckJXemZIRWVHc3BMeFlSRDZYbGVaZnYyQTcxYXNuYnA0TEdkZkVzOSUyRlQ4bXRBQU50VTl6eEVwJTJGYzVyUG0yckc4JTJCdWYxY1IyRUxpRjJMZmdMUHBjVGJ5ZGViWm1pVnlDMm9MYk1IZ0s0ZnclM0QlM0Q; SPC_IA=-1; SPC_EC=-; SPC_U=-; SPC_CT_9dded1e5=\"1608742175.mW9t14ADSirVgObeUhTwhe3f9AqqIQ7iN3zDpvDWk44=\"; SPC_R_T_ID=\"IZ7oKffzhOKFZU8CbnecYBlxt5QlArtcOWwUARAiYhvYKWNynrTcdcjQbjArAHK1OBmXgYAwCH7dJ1QBZbH34lkGSgeF6aZHbG5HlQBzePM=\"; SPC_T_IV=\"eKg8VkyRK15fW1g7F3icMw==\"; SPC_R_T_IV=\"eKg8VkyRK15fW1g7F3icMw==\"; SPC_T_ID=\"IZ7oKffzhOKFZU8CbnecYBlxt5QlArtcOWwUARAiYhvYKWNynrTcdcjQbjArAHK1OBmXgYAwCH7dJ1QBZbH34lkGSgeF6aZHbG5HlQBzePM=\"; SPC_CT_350fb09c=\"1608742359.us/Ga6pi4vCNOJOLuhL/fhd/5QmkoTVmwPIq03XpUPc=\"");
                client.DefaultRequestHeaders.Add("dnt", "1");
                //client.DefaultRequestHeaders.Add("if-none-match", "49d26da40e73b081c80cb06cab0a8f49");
                client.DefaultRequestHeaders.Add("if-none-match-", "55b03-62aaa20face6036b598f81bc4f0f17f8");
                client.DefaultRequestHeaders.Add("referer", "https://shopee.vn/Thi%E1%BA%BFt-B%E1%BB%8B-%C4%90i%E1%BB%87n-T%E1%BB%AD-cat.2365");
                client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
                client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
                client.DefaultRequestHeaders.Add("user-agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66");
                client.DefaultRequestHeaders.Add("x-api-source", "pc");
                client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
                client.DefaultRequestHeaders.Add("x-shopee-language", "vi");


                int errorLoop = 0;
                for (int i = 0; i < numberloop; i++)
                {
                    uriBuilder.Query = $"by={by}&limit={limit}&match_id={categoryid}&newest={offset}&order={order}&page_type={pageType}&version=2";

                    result = client.GetAsync(uriBuilder.Uri).Result;

                    if (result.IsSuccessStatusCode && result.Content.Headers.ContentLength > 0)
                    {
                        productResult = JsonConvert.DeserializeObject<ProductResult>(result.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        productResult = null;
                    }

                    if (result.StatusCode == System.Net.HttpStatusCode.OK && productResult != null && productResult.items.Count > 0)
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
                        Console.WriteLine($"lan lap {i+1}, offset: {offset}");
                    }
                    else
                    {
                        Console.WriteLine($"loi tai lan lap thu {i+1}");
                        errorLoop++;
                        if (errorLoop > 5)
                        {
                            break;
                        }
                        Thread.Sleep(10000);
                        continue;
                    }
                    offset += limit;
                    errorLoop = 0;
                    Thread.Sleep(2000);
                }
            }

        }

        /// <summary>
        /// hàm thực hiện lấy ra shopid, titemid trong mongo, rồi thực hiện lấy cmt theo list product vừa lấy
        /// </summary>
        /// <returns></returns>
        public async Task GetCommentOfProduct()
        {
            IMongoClient client = new MongoClient("mongodb://localhost:27017/");
            IMongoDatabase database = client.GetDatabase("commentInfo");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("product");

            // Created with Studio 3T, the IDE for MongoDB - https://studio3t.com/

            BsonDocument filter = new BsonDocument();

            BsonDocument projection = new BsonDocument();

            projection.Add("shopid", 1.0);
            projection.Add("itemid", 1.0);
            projection.Add("_id", 0.0);


            var options = new FindOptions<BsonDocument>()
            {
                Projection = projection
            };
            using (var cursor = await collection.FindAsync(filter, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    int numberProduct = 0;
                    foreach (BsonDocument document in batch)
                    {
                        Console.WriteLine("------------------------------");
                        Console.WriteLine($"san pham thu {numberProduct}");
                        Console.WriteLine("------------------------------");
                        GetProductModal getProductModal = BsonSerializer.Deserialize<GetProductModal>(document);
                        GetCmt(itemid: getProductModal.itemid, shopid: getProductModal.shopid);
                        numberProduct++;
                    }
                }
            }
        }
        
        /// <summary>
        /// lọc đi những sản phẩm bị trùng
        /// </summary>
        /// <returns></returns>
        public async Task RemoveDuplicateProduct()
        {
            IMongoClient client = new MongoClient("mongodb://localhost:27017/");
            IMongoDatabase database = client.GetDatabase("commentInfo");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("product");

            // Created with Studio 3T, the IDE for MongoDB - https://studio3t.com/

            BsonDocument filter = new BsonDocument();

            BsonDocument projection = new BsonDocument();
            List<ProductIdModal> lstProducts = new List<ProductIdModal>();
            List<string> lstProductRemove = new List<string>();

            projection.Add("itemid", 1.0);

            var options = new FindOptions<BsonDocument>()
            {
                Projection = projection
            };

            using (var cursor = await collection.FindAsync(filter, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        ProductIdModal product = BsonSerializer.Deserialize<ProductIdModal>(document);
                        if (!lstProducts.Any(x => x.itemid == product.itemid))
                        {
                            lstProducts.Add(product);
                        } else
                        {
                            lstProductRemove.Add(product.id.ToString());
                            FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Eq("_id",product.id);
                            var result = collection.DeleteOne(filterDefinition);
                        }
                    }

                }
            }
        }
        /// <summary>
        /// lọc đi những comment bị trùng
        /// </summary>
        /// <returns></returns>
        public async Task RemoveDuplicateComment()
        {
            IMongoClient client = new MongoClient("mongodb://localhost:27017/");
            IMongoDatabase database = client.GetDatabase("commentInfo");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("comment");

            // Created with Studio 3T, the IDE for MongoDB - https://studio3t.com/

            BsonDocument filter = new BsonDocument();

            BsonDocument projection = new BsonDocument();

            List<GetCommentModal> lstComments = new List<GetCommentModal>();

            List<string> lstCommentRemove = new List<string>();
            projection.Add("cmtid", 1.0);

            var options = new FindOptions<BsonDocument>()
            {
                Projection = projection
            };

            using (var cursor = await collection.FindAsync(filter, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        GetCommentModal comment = BsonSerializer.Deserialize<GetCommentModal>(document);
                        if (!lstComments.Any(x => x.cmtid == comment.cmtid))
                        {
                            lstComments.Add(comment);
                        }
                        else
                        {
                            lstCommentRemove.Add(comment.id.ToString());
                            FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Eq("_id", comment.id);
                            var result = collection.DeleteOne(filterDefinition);
                        }
                    }
                }
            }
        }
    }
}
