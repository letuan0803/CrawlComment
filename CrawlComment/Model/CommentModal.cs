using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlComment.Model
{
    class CommentModal
    {
        public class Video
        {
            public string url { get; set; }
            public int duration { get; set; }
            public object upload_time { get; set; }
            public string cover { get; set; }
            public string id { get; set; }
        }

        public class ItemRatingReply
        {
            public object orderid { get; set; }
            public object itemid { get; set; }
            public object cmtid { get; set; }
            public int? ctime { get; set; }
            public object mentioned { get; set; }
            public object rating { get; set; }
            public object editable { get; set; }
            public int? userid { get; set; }
            public object shopid { get; set; }
            public string comment { get; set; }
            public object filter { get; set; }
            public object rating_star { get; set; }
            public object status { get; set; }
            public int? mtime { get; set; }
            public object opt { get; set; }
            public bool? is_hidden { get; set; }
        }

        public class Tag
        {
            public string tag_description { get; set; }
            public int tag_id { get; set; }
        }

        public class TierVariation
        {
            public object images { get; set; }
            public object properties { get; set; }
            public int type { get; set; }
            public string name { get; set; }
            public List<string> options { get; set; }
        }

        public class ProductItem
        {
            public object itemid { get; set; }
            public object welcome_package_info { get; set; }
            public object liked { get; set; }
            public object recommendation_info { get; set; }
            public object bundle_deal_info { get; set; }
            public object price_max_before_discount { get; set; }
            public string image { get; set; }
            public object recommendation_algorithm { get; set; }
            public bool is_cc_installment_payment_eligible { get; set; }
            public int shopid { get; set; }
            public bool can_use_wholesale { get; set; }
            public object group_buy_info { get; set; }
            public string reference_item_id { get; set; }
            public string currency { get; set; }
            public int raw_discount { get; set; }
            public object show_free_shipping { get; set; }
            public object video_info_list { get; set; }
            public List<string> images { get; set; }
            public object is_preferred_plus_seller { get; set; }
            public object price_before_discount { get; set; }
            public bool is_category_failed { get; set; }
            public int show_discount { get; set; }
            public int cmt_count { get; set; }
            public object view_count { get; set; }
            public int catid { get; set; }
            public object upcoming_flash_sale { get; set; }
            public bool is_official_shop { get; set; }
            public string brand { get; set; }
            public object price_min { get; set; }
            public int liked_count { get; set; }
            public bool can_use_bundle_deal { get; set; }
            public bool show_official_shop_label { get; set; }
            public object coin_earn_label { get; set; }
            public int is_snapshot { get; set; }
            public object price_min_before_discount { get; set; }
            public int cb_option { get; set; }
            public object sold { get; set; }
            public int stock { get; set; }
            public int status { get; set; }
            public object price_max { get; set; }
            public object add_on_deal_info { get; set; }
            public object is_group_buy_item { get; set; }
            public object flash_sale { get; set; }
            public object modelid { get; set; }
            public object price { get; set; }
            public object shop_location { get; set; }
            public object item_rating { get; set; }
            public bool show_official_shop_label_in_title { get; set; }
            public List<TierVariation> tier_variations { get; set; }
            public object is_adult { get; set; }
            public string discount { get; set; }
            public int flag { get; set; }
            public bool is_non_cc_installment_payment_eligible { get; set; }
            public bool has_lowest_price_guarantee { get; set; }
            public object snapshotid { get; set; }
            public object has_group_buy_stock { get; set; }
            public object preview_info { get; set; }
            public int welcome_package_type { get; set; }
            public object exclusive_price_info { get; set; }
            public string name { get; set; }
            public int ctime { get; set; }
            public List<object> wholesale_tier_list { get; set; }
            public bool show_shopee_verified_label { get; set; }
            public object show_official_shop_label_in_normal_position { get; set; }
            public string item_status { get; set; }
            public object shopee_verified { get; set; }
            public object hidden_price_display { get; set; }
            public object size_chart { get; set; }
            public int item_type { get; set; }
            public object shipping_icon_type { get; set; }
            public List<int> label_ids { get; set; }
            public object service_by_shopee_flag { get; set; }
            public string model_name { get; set; }
            public object badge_icon_type { get; set; }
            public object historical_sold { get; set; }
            public string transparent_background_image { get; set; }
        }

        public class Rating
        {
            public object itemid { get; set; }
            public int rating { get; set; }
            public object liked { get; set; }
            public List<Video> videos { get; set; }
            public int shopid { get; set; }
            public object show_reply { get; set; }
            public int rating_star { get; set; }
            public int? like_count { get; set; }
            public int mtime { get; set; }
            public List<object> mentioned { get; set; }
            public ItemRatingReply ItemRatingReply { get; set; }
            public bool is_hidden { get; set; }
            public string author_portrait { get; set; }
            public object detailed_rating { get; set; }
            public object orderid { get; set; }
            public object cmtid { get; set; }
            public bool exclude_scoring_due_low_logistic { get; set; }
            public int? editable_date { get; set; }
            public int opt { get; set; }
            public int status { get; set; }
            public string author_username { get; set; }
            public List<Tag> tags { get; set; }
            public List<string> images { get; set; }
            public object delete_operator { get; set; }
            public int editable { get; set; }
            public bool anonymous { get; set; }
            public object loyalty_info { get; set; }
            public int ctime { get; set; }
            public List<ProductItem> product_items { get; set; }
            public int author_shopid { get; set; }
            public int userid { get; set; }
            public string comment { get; set; }
            public int filter { get; set; }
            public bool sync_to_social { get; set; }
            public object delete_reason { get; set; }
        }

        public class ItemRatingSummary
        {
            public int rcount_with_context { get; set; }
            public int rcount_with_media { get; set; }
            public List<int> rating_count { get; set; }
            public int rating_total { get; set; }
            public int rcount_with_image { get; set; }
        }

        public class Data
        {
            public List<Rating> ratings { get; set; }
            public ItemRatingSummary item_rating_summary { get; set; }
        }

        public class CommentResult
        {
            public string version { get; set; }
            public Data data { get; set; }
            public object error_msg { get; set; }
            public int error { get; set; }
        }


    }
}
