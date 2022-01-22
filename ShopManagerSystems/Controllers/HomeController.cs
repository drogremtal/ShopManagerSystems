using Microsoft.AspNetCore.Mvc;
using ShopManagerSystems.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using DataLayer.DataLayer;
using AutoMapper;
using ShopManagerSystems.ViewModel;
using BusinessLayer.Service;
using Service.DTO;

namespace ShopManagerSystems.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _DB;
        private readonly IDTOService _Service;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext dBContext, IDTOService dTOService)
        {
            _logger = logger;
            _DB = dBContext;
            _Service = dTOService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserList()
        {
            var Users = _Service.GetUsers();

            var mapper = new MapperConfiguration(cgf => cgf.CreateMap<UserDTO, UserListViewModel>()).CreateMapper();

            var UsersList = mapper.Map<IEnumerable<UserDTO>, List<UserListViewModel>>(Users);

            return View(UsersList);
        }
        public IActionResult CreateUser()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateUser(UserCreateViewModel user)
        {
            var mapper = new MapperConfiguration(cfg=>cfg.CreateMap<UserCreateViewModel,UserDTO>()).CreateMapper();

            var userDto = mapper.Map<UserCreateViewModel, UserDTO>(user);
            _Service.CreateUser(userDto);

            return RedirectToAction("UserList");
        }

        public IActionResult Details(int id)
        {
            var userDetailsViewModel = from u in _DB.User
                                       join ui in _DB.UserInformation
                                       on u.Id equals ui.UserID

                                       into UserInfo
                                       from item in UserInfo.DefaultIfEmpty()
                                       where u.Id == id
                                       select new UserDetailsViewModel
                                       {
                                           UserId = u.Id,
                                           LastName = u.LastName,
                                           FirstName = u.FirstName,
                                           PatronicName = u.PatronicName,
                                           BirthDate = item.BirthDate,
                                           Email = item.Email,
                                           PhoneNum = item.PhoneNum
                                       };
            if (!userDetailsViewModel.Any())
            {
                return View("UserList");
            }

            return View(userDetailsViewModel.FirstOrDefault());
        }

        public IActionResult Edit(int id)
        {


            var userDetailsViewModel = from u in _DB.User
                                       join ui in _DB.UserInformation
                                       on u.Id equals ui.UserID

                                       into UserInfo
                                       from item in UserInfo.DefaultIfEmpty()
                                       where u.Id == id
                                       select new UserDetailEditViewModel
                                       {
                                           UserId = u.Id,
                                           LastName = u.LastName,
                                           FirstName = u.FirstName,
                                           PatronicName = u.PatronicName,
                                           BirthDate = item.BirthDate,
                                           Email = item.Email,
                                           PhoneNum = item.PhoneNum
                                       };

            if (!userDetailsViewModel.Any())
            {
                return View("UserList");
            }

            return View(userDetailsViewModel.FirstOrDefault());
        }

        [HttpPost]
        public IActionResult Edit(UserDetailEditViewModel userDetailsViewModel)
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<UserDetailEditViewModel, User>()
                     .ForMember(pr => pr.Id, opt => opt.MapFrom(q => q.UserId))
                     .ForMember(pr => pr.LastName, opt => opt.MapFrom(q => q.LastName))
                     .ForMember(pr => pr.FirstName, opt => opt.MapFrom(q => q.FirstName))
                     .ForMember(pr => pr.PatronicName, opt => opt.MapFrom(q => q.PatronicName));

                cfg.CreateMap<UserDetailEditViewModel, UserInformation>()
                .ForMember(pr => pr.BirthDate, opt => opt.MapFrom(q => q.BirthDate))
                .ForMember(pr => pr.Email, opt => opt.MapFrom(q => q.Email))
                .ForMember(pr => pr.UserID, opt => opt.MapFrom(q => q.UserId))
                .ForMember(pr => pr.PhoneNum, opt => opt.MapFrom(q => q.PhoneNum));
            });

            var map = new Mapper(mapConfig);

            var user = map.Map<UserDetailEditViewModel, User>(userDetailsViewModel);
            var userInformation = map.Map<UserDetailEditViewModel, UserInformation>(userDetailsViewModel);

            _DB.User.Update(user);

            _DB.SaveChanges();
            _DB.UserInformation.Update(userInformation);
            _DB.SaveChanges();

            return RedirectToAction("Details", new { id = userDetailsViewModel.UserId });
        }

        public IActionResult Delete(int id)
        {
            var user = _DB.User.Find(id);
            if (user != null)
            {
                _DB.User.Remove(user);
                _DB.SaveChanges();
            }

            var userinfo = _DB.UserInformation.Where(q => q.UserID == id).FirstOrDefault();

            if (userinfo != null)
            {
                _DB.UserInformation.Remove(userinfo);
                _DB.SaveChanges();
            }

            return RedirectToAction("UserList");
        }

        [HttpPost]
        public IActionResult CreateCheck(Check check)
        {
            _DB.Check.Add(check);
            _DB.SaveChanges();
            return RedirectToAction("Check", new { id = check.Id });
        }

        public IActionResult CreateCheck(int id)
        {
            return View(new Check() { UserId = id});
        }

        public IActionResult Check(int id)
        {
            var check = _DB.Check.Find(id);
            return View(check);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




    }
}