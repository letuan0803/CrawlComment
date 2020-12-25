using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlComment.Model
{
    class GetProductModal
    {
        public Guid? id { get; set; }
        public long itemid { get; set; }
        public int shopid { get; set; }
    }

    class ProductIdModal
    {
        public object id { get; set; }
        public long itemid { get; set; }
    }
}
