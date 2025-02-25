using AssignmentPS42054.Models;
using Microsoft.Data.SqlClient;

namespace AssignmentPS42054.DAL
{
    public class CategoryDAL
    {
        private readonly string _connectionString;
        public CategoryDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IEnumerable<Category> GetAllCats()
        {
            var cat = new List<Category>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select * from dbo.Category", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category
                    {
                        Id = (int)reader["category_id"],
                        Name = (string)reader["name_category"],
                        Cat_Img = reader["img_category"] != DBNull.Value ? (string)reader["img_category"] : string.Empty
                    });
                }
            }
            return cat;
        }
        public void AddCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into dbo.Category(name_category, img_category) values(@Name, @Img)", conn);
                cmd.Parameters.AddWithValue("@Name", category.Name);
                cmd.Parameters.AddWithValue("@Img", category.Cat_Img ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("update dbo.Category set name_category = @Name, img_category = @Img where category_id = @id", conn);

                cmd.Parameters.AddWithValue("@id", category.Id);
                cmd.Parameters.AddWithValue("@Name", category.Name);
                cmd.Parameters.AddWithValue("@Img", category.Cat_Img);
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteCategory(int categoryId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Chỉ cần sử dụng DELETE mà không cần SET
                SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Category WHERE category_id = @id", conn);

                // Thêm tham số cho id
                cmd.Parameters.AddWithValue("@id", categoryId);

                // Thực hiện lệnh
                cmd.ExecuteNonQuery();
            }
        }
        public Category GetCatById(int catID)
        {
            Category cat = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Sửa từ "Id" thành "category_id" để phù hợp với tên cột trong bảng Category
                SqlCommand cmd = new SqlCommand("SELECT * FROM Category WHERE category_id = @catID", conn);
                cmd.Parameters.AddWithValue("@catID", catID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cat = new Category
                    {
                        Id = (int)reader["category_id"],
                        Name = (string)reader["name_category"],
                        Cat_Img = reader["img_category"] != DBNull.Value ? (string)reader["img_category"] : string.Empty
                    };
                }
            }
            return cat;
        }



    }
}
