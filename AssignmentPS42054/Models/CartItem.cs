namespace AssignmentPS42054.Models
{
    public class CartItem
    {
       
            public int Id { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            //public string Image { get; set; }
            public string DetailImage { get; set; }

    }
}
