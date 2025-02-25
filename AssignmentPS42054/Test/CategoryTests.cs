//using NUnit.Framework;
//using AssignmentPS42054.DAL;
//using AssignmentPS42054.Models;
//using Microsoft.Extensions.Configuration;
//using System.Collections.Generic;
//using System.Linq;

//namespace AssignmentPS42054.Test
//{
//    [TestFixture]
//    public class CategoryTests
//    {
//        private CategoryDAL _categoryDAL;
//        private IConfiguration _configuration;

//        [SetUp]
//        public void Setup()
//        {
//            var config = new Dictionary<string, string>
//            {
//                {"ConnectionStrings:DefaultConnection", "Data Source=GIANGDEPTRAI;Initial Catalog=asm_net103;Trusted_Connection=True;TrustServerCertificate=True;"}
//            };

//            var configuration = new ConfigurationBuilder()
//                .AddInMemoryCollection(config)
//                .Build();

//            _configuration = configuration;
//            _categoryDAL = new CategoryDAL(_configuration);
//        }

//        [Test]
//        public void GetAllCategories_ShouldReturnListOfCategories()
//        {
//            var categories = _categoryDAL.GetAllCats().ToList();
//            Assert.IsNotNull(categories, "Category list should not be null.");
//            Assert.IsInstanceOf<List<Category>>(categories, "Should return a list of Categories.");
//        }

//        [Test]
//        public void AddCategory_ShouldInsertCategory()
//        {
//            var category = new Category
//            {
//                Name = "Test Category",
//                Cat_Img = "test_image.jpg"
//            };
//            _categoryDAL.AddCategory(category);
//            var categories = _categoryDAL.GetAllCats().ToList();
//            Assert.IsTrue(categories.Any(c => c.Name == "Test Category"), "Category should be added successfully.");
//        }

//        [Test]
//        public void GetCategoryById_ShouldReturnCorrectCategory()
//        {
//            var category = new Category
//            {
//                Name = "Category By ID",
//                Cat_Img = "image.jpg"
//            };
//            _categoryDAL.AddCategory(category);
//            var allCategories = _categoryDAL.GetAllCats().ToList();
//            var insertedCategory = allCategories.FirstOrDefault(c => c.Name == "Category By ID");
//            var retrievedCategory = _categoryDAL.GetCatById(insertedCategory.Id);
//            Assert.IsNotNull(retrievedCategory, "Category should be retrieved by ID.");
//            Assert.AreEqual(insertedCategory.Name, retrievedCategory.Name, "Category names should match.");
//        }

//        [Test]
//        public void UpdateCategory_ShouldModifyExistingCategory()
//        {
//            var category = new Category
//            {
//                Name = "Category To Update",
//                Cat_Img = "initial_image.jpg"
//            };
//            _categoryDAL.AddCategory(category);
//            var allCategories = _categoryDAL.GetAllCats().ToList();
//            var insertedCategory = allCategories.FirstOrDefault(c => c.Name == "Category To Update");

//            insertedCategory.Cat_Img = "updated_image.jpg";
//            _categoryDAL.UpdateCategory(insertedCategory);

//            var updatedCategory = _categoryDAL.GetCatById(insertedCategory.Id);
//            Assert.AreEqual("updated_image.jpg", updatedCategory.Cat_Img, "Category image should be updated.");
//        }

//        [Test]
//        public void DeleteCategory_ShouldRemoveCategory()
//        {
//            var category = new Category
//            {
//                Name = "Category To Delete",
//                Cat_Img = "delete_image.jpg"
//            };
//            _categoryDAL.AddCategory(category);
//            var allCategories = _categoryDAL.GetAllCats().ToList();
//            var insertedCategory = allCategories.FirstOrDefault(c => c.Name == "Category To Delete");

//            _categoryDAL.DeleteCategory(insertedCategory.Id);
//            var deletedCategory = _categoryDAL.GetCatById(insertedCategory.Id);
//            Assert.IsNull(deletedCategory, "Category should be deleted.");
//        }
//    }
//}
