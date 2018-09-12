using System;

namespace YellowShop.Models
{
    public class PaginationModel
    {
        public int TotalItems { get; set; }

        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); // Expression body property
    }
}
