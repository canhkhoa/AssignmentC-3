using AssignmentPS42054.DAL;
using AssignmentPS42054.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentPS42054.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AccountDAL _accountDAL;

        public UserController(AccountDAL accountDAL)
        {
            _accountDAL = accountDAL;
        }
        public IActionResult Index()
        {
            var users = _accountDAL.GetAllUsers();
            return View(users);
        }
        public IActionResult Create()
        {
            var user = new Users(); 
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Users user)
        {
            if (ModelState.IsValid)
            {
                
                _accountDAL.AddUser(user);
                return RedirectToAction("Index");
            }
            else
            {
             
                return View(user);
            }
        }
        public IActionResult Edit(int id)
        {
            var user = _accountDAL.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Users user)
        {
            if (ModelState.IsValid)
            {

                _accountDAL.UpdateUser(user);
                return RedirectToAction("Index");
            }
            else
            {

                return View(user);
            }
        }

        // Hiển thị form xóa người dùng
        public IActionResult Delete(int id)
        {
            var user = _accountDAL.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Xóa người dùng
            _accountDAL.DeleteUser(id);
            return RedirectToAction("Index");
        }
    }
}

