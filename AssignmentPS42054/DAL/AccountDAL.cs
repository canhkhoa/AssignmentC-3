using AssignmentPS42054.Models;
using Microsoft.Data.SqlClient;

namespace AssignmentPS42054.DAL
{
    public class AccountDAL
    {
        private readonly string _connectionString;
        public AccountDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public bool Register(string Name, string Password, string Email, int Mobile)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE user_name = @Name", conn);
                checkCmd.Parameters.AddWithValue("@Name", Name);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    return false; 
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Users (user_name, password, email, mobile) VALUES (@Name, @Password, @Email, @Mobile)", conn);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@Email",Email);
                cmd.Parameters.AddWithValue("@Mobile", Mobile);
                cmd.ExecuteNonQuery();
                return true; 
            }
        }
        public Users Login(string Name, string Password)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Password))
            {
                throw new ArgumentException("Name or Password cannot be null or empty.");
            }

            Users user = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT user_name, password, role_id FROM Users WHERE user_name = @Name AND password = @Password", conn);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Password", Password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new Users
                    {
                        Name = (string)reader["user_name"],
                        Pass = (string)reader["password"],
                        roleID = (int)reader["role_id"]
                    };
                }
            }
            return user;
        }
        public IEnumerable<Users> GetAllUsers()
        {
            var user = new List<Users>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select * from dbo.Users", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user.Add(new Users
                    {
                        Id = (int)reader["user_id"],
                        FName = reader["fullname"] != DBNull.Value ? (string)reader["fullname"] : string.Empty,
                        Address = reader["address"] != DBNull.Value ? (string)reader["address"] : string.Empty,
                        roleID = reader["role_id"] != DBNull.Value ? (int?)reader["role_id"] : null,
                        Name = (string)reader["user_name"],
                        Pass = (string)reader["password"],
                        Email = reader["email"] != DBNull.Value ? (string)reader["email"] : string.Empty,
                        Mobile = (int)reader["mobile"]
                    });
                }
            }
            return user;
        }
        public Users GetUserById(int userId)
        {
            Users user = new Users();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE user_id = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new Users
                    {
                        Id = (int)reader["user_id"],
                        FName = reader["fullname"] != DBNull.Value ? (string)reader["fullname"] : string.Empty,
                        Address = reader["address"] != DBNull.Value ? (string)reader["address"] : string.Empty,
                        roleID = reader["role_id"] != DBNull.Value ? (int?)reader["role_id"] : null,
                        Name = (string)reader["user_name"],
                        Pass = (string)reader["password"],
                        Email = reader["email"] != DBNull.Value ? (string)reader["email"] : string.Empty,
                        Mobile = (int)reader["mobile"]
                    };
                }
            }
            return user;
        }
        public int GetIDbyUser(string userName)
        {
            int userId = -1; // Khởi tạo với giá trị không hợp lệ, để kiểm tra sau này

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT user_id FROM Users WHERE user_name = @userName", conn);
                cmd.Parameters.AddWithValue("@userName", userName);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userId = (int)reader["user_id"]; // Gán giá trị của user_id nếu tìm thấy
                }
            }

            return userId; // Trả về userId, nếu không tìm thấy sẽ trả về giá trị -1
        }

        public void AddUser(Users user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Users (fullname, address, role_id, user_name, password, email, mobile) " +
                    "VALUES (@FName, @Address, @roleID, @Name, @Pass, @Email, @Mobile)", conn);

                cmd.Parameters.AddWithValue("@FName", user.FName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", user.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@roleID", user.roleID.HasValue ? user.roleID.Value : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Pass", user.Pass);
                cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Mobile", user.Mobile);

                cmd.ExecuteNonQuery();  
            }
        }

        public void UpdateUser(Users user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Users SET fullname = @FName, address = @Address, role_id = @roleID, user_name = @Name, password = @Pass, email = @Email, mobile = @Mobile " +
                    "WHERE user_id = @Id", conn);

                cmd.Parameters.AddWithValue("@Id", user.Id); // Cập nhật dựa trên Id của người dùng
                cmd.Parameters.AddWithValue("@FName", user.FName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", user.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@roleID", user.roleID.HasValue ? user.roleID.Value : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Pass", user.Pass);
                cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Mobile", user.Mobile);

                cmd.ExecuteNonQuery();  // Thực thi câu lệnh SQL để cập nhật
            }
        }

        public void DeleteUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE user_id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", userId);  // Tham số là Id của người dùng cần xóa

                cmd.ExecuteNonQuery();  // Thực thi câu lệnh SQL để xóa bản ghi
            }
        }





    }
}
