namespace Dialog.Models.Blog
{
    public class PostsTags
    {
        public string Id { get; set; }

        public string PostId { get; set; }
        public virtual Post Post { get; set; }

        public string TagId { get; set; }

        public virtual Tag Tag { get; set; }
    }
}