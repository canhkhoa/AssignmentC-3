using AssignmentPS42054.Models;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace AssignmentPS42054.DAL
{
    public class ProductDAL
    {
        private readonly string _connectionString;

        public ProductDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Product", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Id = (int)reader["product_id"],
                        Name = (string)reader["name_product"],
                        Price = Convert.ToSingle(reader["price"]),
                        Quantity = (int)reader["quantity"],
                        CreateDate = (DateTime)reader["create_date"],
                        Description = reader["description"] != DBNull.Value ? (string)reader["description"] : string.Empty,
                        ProductImage = reader["img"] != DBNull.Value ? (string)reader["img"] : string.Empty,
                        CategoryId = (int)reader["category_id"],
                        Image1 = reader["imgSub1"] != DBNull.Value ? (string)reader["imgSub1"] : string.Empty,
                        Image2 = reader["imgSub2"] != DBNull.Value ? (string)reader["imgSub2"] : string.Empty,
                        Image3 = reader["imgSub3"] != DBNull.Value ? (string)reader["imgSub3"] : string.Empty,
                        Review = reader["review"] != DBNull.Value ? (int)reader["review"] : 0
                    });
                }
            }
            return products;
        }

        public void AddProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO dbo.Product(name_product, price, quantity, create_date, description, img, category_id, imgSub1, imgSub2, imgSub3, review) " +
                    "VALUES(@Name, @Price, @Quantity, @CreateDate, @Description, @Img, @CategoryId, @Img1, @Img2, @Img3, @Review)", conn);

                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@CreateDate", product.CreateDate < (DateTime)SqlDateTime.MinValue ? DateTime.Now : product.CreateDate); // Kiểm tra CreateDate
                cmd.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Img", product.ProductImage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                cmd.Parameters.AddWithValue("@Img1", product.Image1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Img2", product.Image2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Img3", product.Image3 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Review", product.Review);

                cmd.ExecuteNonQuery();
            }
        }


        public void UpdateProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE dbo.Product SET name_product = @Name, price = @Price, quantity = @Quantity, create_date = @CreateDate, " +
                    "description = @Description, img = @Img, category_id = @CategoryId, imgSub1 = @Img1, imgSub2 = @Img2, imgSub3 = @Img3, review = @Review " +
                    "WHERE product_id = @Id", conn);

                cmd.Parameters.AddWithValue("@Id", product.Id);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@CreateDate", product.CreateDate < (DateTime)SqlDateTime.MinValue ? DateTime.Now : product.CreateDate); // Kiểm tra CreateDate
                cmd.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Img", product.ProductImage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                cmd.Parameters.AddWithValue("@Img1", product.Image1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Img2", product.Image2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Img3", product.Image3 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Review", product.Review);

                cmd.ExecuteNonQuery();
            }
        }


        public void DeleteProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Product WHERE product_id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", product.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public Product GetProductById(int productId)
        {
            Product product = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Product WHERE product_id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", productId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product
                    {
                        Id = (int)reader["product_id"],
                        Name = (string)reader["name_product"],
                        Price = Convert.ToSingle(reader["price"]),
                        Quantity = (int)reader["quantity"],
                        CreateDate = (DateTime)reader["create_date"],
                        Description = reader["description"] != DBNull.Value ? (string)reader["description"] : string.Empty,
                        ProductImage = reader["img"] != DBNull.Value ? (string)reader["img"] : string.Empty,
                        CategoryId = (int)reader["category_id"],
                        Image1 = reader["imgSub1"] != DBNull.Value ? (string)reader["imgSub1"] : string.Empty,
                        Image2 = reader["imgSub2"] != DBNull.Value ? (string)reader["imgSub2"] : string.Empty,
                        Image3 = reader["imgSub3"] != DBNull.Value ? (string)reader["imgSub3"] : string.Empty,
                        Review = reader["review"] != DBNull.Value ? (int)reader["review"] : 0
                    };
                }
            }
            return product;
        }
        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            List<Product> products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Product WHERE category_id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", categoryId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) // Sử dụng while để đọc nhiều hàng
                {
                    Product product = new Product
                    {
                        Id = (int)reader["product_id"],
                        Name = (string)reader["name_product"],
                        Price = Convert.ToSingle(reader["price"]),
                        Quantity = (int)reader["quantity"],
                        CreateDate = (DateTime)reader["create_date"],
                        Description = reader["description"] != DBNull.Value ? (string)reader["description"] : string.Empty,
                        ProductImage = reader["img"] != DBNull.Value ? (string)reader["img"] : string.Empty,
                        CategoryId = (int)reader["category_id"],
                        Image1 = reader["imgSub1"] != DBNull.Value ? (string)reader["imgSub1"] : string.Empty,
                        Image2 = reader["imgSub2"] != DBNull.Value ? (string)reader["imgSub2"] : string.Empty,
                        Image3 = reader["imgSub3"] != DBNull.Value ? (string)reader["imgSub3"] : string.Empty,
                        Review = reader["review"] != DBNull.Value ? (int)reader["review"] : 0
                    };
                    products.Add(product); // Thêm sản phẩm vào danh sách
                }
            }
            return products;
        }

    }
}
