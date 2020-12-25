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
            // lấy 100*50 sản phẩm phổ biến nhất của mặt hàng thiết bị điện tử, rồi lấy danh sách sản phẩm vào db
            getComment.GetProduct(numberloop: 100, categoryid: 2365);

            // lấy ra list sản phẩm trong db, rồi thực hiện lấy tất cmt của sản phẩm trong list
            getComment.GetCommentOfProduct().Wait();

            // thực hiện lọc sản phẩm trùng theo productid
            getComment.RemoveDuplicateProduct().Wait();

            // thực hiện lọc comment trùng theo cmtid
            getComment.RemoveDuplicateComment().Wait();
        }
    }
}
