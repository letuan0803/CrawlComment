using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlComment
{
    class Program
    {

        static void Main(string[] args)
        {
            GetComment getComment = new GetComment();
            getComment.GetProduct(limit: 5);
            //getComment.GetCmt();
        }
    }
}
