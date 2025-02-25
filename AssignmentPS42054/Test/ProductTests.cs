//using NUnit.Framework;
//using AssignmentPS42054.DAL;
//using AssignmentPS42054.Models;
//using Microsoft.Extensions.Configuration;
//using Moq;
//using System.Collections.Generic;
//using Microsoft.Data.SqlClient;
//using System.Linq;

//namespace AssignmentPS42054.Test
//{
//    [TestFixture]
//    public class ProductTests
//    {
//        private ProductDAL _productDAL;
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
//            _productDAL = new ProductDAL(_configuration);
//        }

//        [Test]
//        public void GetAllProducts_ShouldReturnListOfProducts()
//        {
//            var products = _productDAL.GetAllProducts().ToList();
//            Assert.IsNotNull(products, "Product list should not be null.");
//            Assert.IsInstanceOf<List<Product>>(products, "Should return a list of Products.");
//        }

//        [Test]
//        public void AddProduct_failInsertProduct()
//        {
//            var product = new Product
//            {
//                Name = "Test Product",
//                Price = 10.99f,
//                Quantity = 5,
//                CreateDate = System.DateTime.Now,
//                Description = "Test description",
//                CategoryId = 1,
//                Review = 5
//            };
//            _productDAL.AddProduct(product);
//            var products = _productDAL.GetAllProducts().ToList();
//            Assert.IsTrue(products.Any(p => p.Name == "Test Product"), "Product should be added successfully.");
//        }
//        [Test]
//        public void AddProduct_ShouInsertProduct()
//        {
//            var product = new Product
//            {
//                Name = "Test Product",
//                Price = 10.99f,
//                Quantity = 5,
//                CreateDate = System.DateTime.Now,
//                Description = "Test description",
//                CategoryId = 1,
//                ProductImage = "image.jpg",
//                Image1 = "img1.jpg",
//                Image2 = "img2.jpg",
//                Image3 = "img3.jpg",
//                Review = 5
//            };
//            _productDAL.AddProduct(product);
//            var products = _productDAL.GetAllProducts().ToList();
//            Assert.IsTrue(products.Any(p => p.Name == "Test Product"), "Product should be added successfully.");
//        }
//        [Test]
//        public void GetProductById_ShouldReturnCorrectProduct()
//        {
//            var product = new Product
//            {
//                Name = "Product By ID",
//                Price = 15.99f,
//                Quantity = 10,
//                CreateDate = System.DateTime.Now,
//                Description = "Test product",
//                CategoryId = 2,
//                Review = 1
//            };
//            _productDAL.AddProduct(product);
//            var allProducts = _productDAL.GetAllProducts().ToList();
//            var insertedProduct = allProducts.FirstOrDefault(p => p.Name == "Product By ID");
//            var retrievedProduct = _productDAL.GetProductById(insertedProduct.Id);
//            Assert.IsNotNull(retrievedProduct, "Product should be retrieved by ID.");
//            Assert.AreEqual(insertedProduct.Name, retrievedProduct.Name, "Product names should match.");
//        }

//        [Test]
//        public void UpdateProduct_ShouldModifyExistingProduct_WithValidData()
//        {
//            var product = new Product
//            {
//                Name = "Product To Update",
//                Price = 20.99f,
//                Quantity = 8,
//                CreateDate = System.DateTime.Now,
//                Description = "Initial description",
//                CategoryId = 3
//            };
//            _productDAL.AddProduct(product);
//            var allProducts = _productDAL.GetAllProducts().ToList();
//            var insertedProduct = allProducts.FirstOrDefault(p => p.Name == "Product To Update");

//            insertedProduct.Description = "Updated description";
//            insertedProduct.Price = 25.99f;
//            _productDAL.UpdateProduct(insertedProduct);

//            var updatedProduct = _productDAL.GetProductById(insertedProduct.Id);
//            Assert.AreEqual("Updated description", updatedProduct.Description, "Product description should be updated.");
//            Assert.AreEqual(25.99f, updatedProduct.Price, "Product price should be updated.");
//        }

//        [Test]
//        public void UpdateProduct_ShouldFail_WithInvalidData()
//        {
//            var product = new Product
//            {
//                Name = "Invalid Update",
//                Price = 30.99f,
//                Quantity = 12,
//                CreateDate = System.DateTime.Now,
//                Description = "Invalid test case",
//                CategoryId = 5
//            };
//            _productDAL.AddProduct(product);
//            var allProducts = _productDAL.GetAllProducts().ToList();
//            var insertedProduct = allProducts.FirstOrDefault(p => p.Name == "Invalid Update");

//            insertedProduct.Name = null;
//            insertedProduct.Price = -5; // Invalid price

//            Assert.Throws<Microsoft.Data.SqlClient.SqlException>(() => _productDAL.UpdateProduct(insertedProduct), "Should throw an exception for invalid data.");
//        }

//        [Test]
//        public void DeleteProduct_ShouldRemoveProduct()
//        {
//            var product = new Product
//            {
//                Name = "Product To Delete",
//                Price = 5.99f,
//                Quantity = 3,
//                CreateDate = System.DateTime.Now,
//                Description = "To be deleted",
//                CategoryId = 4
//            };
//            _productDAL.AddProduct(product);
//            var allProducts = _productDAL.GetAllProducts().ToList();
//            var insertedProduct = allProducts.FirstOrDefault(p => p.Name == "Product To Delete");

//            _productDAL.DeleteProduct(insertedProduct);
//            var deletedProduct = _productDAL.GetProductById(insertedProduct.Id);
//            Assert.IsNull(deletedProduct, "Product should be deleted.");
//        }
//    }
//}