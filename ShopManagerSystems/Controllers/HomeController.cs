using Microsoft.AspNetCore.Mvc;
using ShopManagerSystems.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using DataLayer.DataLayer;
using AutoMapper;
using ShopManagerSystems.ViewModel;
using System.Linq;

namespace ShopManagerSystems.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _DB;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext dBContext)
        {
            _logger = logger;
            _DB = dBContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserList()
        {
            var UsersList = _DB.User.ToList();
            return View(UsersList);
        }
        public IActionResult CreateUser()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            _DB.User.Add(user);
            _DB.SaveChanges();
            return RedirectToAction("UserList");
        }

        public IActionResult Details(int id)
        {
            User user = new User();
            user = _DB.User.Find(id);
            if (user == null)
            {
                return RedirectToAction("UserList");
            }

            UserInformation userInformation = new UserInformation();
            userInformation = _DB.UserInformation.Find(id) ?? new UserInformation();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDetailsViewModel>()
               .ForMember(pr => pr.UserId, opt => opt.MapFrom(q => q.Id))
               .ForMember(pr => pr.LastName, opt => opt.MapFrom(q => q.LastName))
               .ForMember(pr => pr.FirstName, opt => opt.MapFrom(q => q.FirstName))
               .ForMember(pr => pr.PatronicName, opt => opt.MapFrom(q => q.Patronic));
                cfg.CreateMap<UserInformation, UserDetailsViewModel>()
                .ForMember(pr => pr.BirthDate, opt => opt.MapFrom(q => q.BirthDate))
                .ForMember(pr => pr.Email, opt => opt.MapFrom(q => q.Email))
                .ForMember(pr => pr.PhoneNum, opt => opt.MapFrom(q => q.PhoneNum));
            });


            var map = new Mapper(config);

            var userDetailsViewModel = map.Map<User, UserDetailsViewModel>(user);
            map.Map(userInformation, userDetailsViewModel);

            return View(userDetailsViewModel);
        }

        public IActionResult Edit(int id)
        {
            //var userDetailsViewModel = _DB.User.GroupJoin(_DB.UserInformation,
            //    u => u.Id,
            //    ui => ui.UserID,
            //    (u,ui) => new UserDetailsViewModel
            //    {
            //        UserId = u.Id,
            //        LastName = u.LastName,
            //        FirstName = u.FirstName,
            //        PatronicName = u.Patronic,
            //        BirthDate = ui.BirthDate,
            //        Email = ui.Email,
            //        PhoneNum = ui.PhoneNum
            //    }).Where(q=>q.UserId == id).FirstOrDefault();

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
                                           PatronicName = u.Patronic,
                                           BirthDate = item.BirthDate,
                                           Email = item.Email,
                                           PhoneNum = item.PhoneNum
                                       };

            if (!userDetailsViewModel.Any())
            {
                return View("UserList");
            }


            //User user = new();
            //if (user == null)
            //{
            //    return RedirectToAction("UserList");
            //}

            //var userDetails = _DB.UserInformation.Find(id) ?? new UserInformation();

            //var config = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<User, UserDetailsViewModel>()
            //   .ForMember(pr => pr.UserId, opt => opt.MapFrom(q => q.Id))
            //   .ForMember(pr => pr.LastName, opt => opt.MapFrom(q => q.LastName))
            //   .ForMember(pr => pr.FirstName, opt => opt.MapFrom(q => q.FirstName))
            //   .ForMember(pr => pr.PatronicName, opt => opt.MapFrom(q => q.Patronic));
            //    cfg.CreateMap<UserInformation, UserDetailsViewModel>()
            //    .ForMember(pr => pr.BirthDate, opt => opt.MapFrom(q => q.BirthDate))
            //    .ForMember(pr => pr.Email, opt => opt.MapFrom(q => q.Email))
            //    .ForMember(pr => pr.PhoneNum, opt => opt.MapFrom(q => q.PhoneNum));
            //});

            //var map = new Mapper(config);


            //var userDetailsViewModel = map.Map<User, UserDetailsViewModel>(user);
            //userDetailsViewModel = map.Map(userDetails, userDetailsViewModel);


            return View(userDetailsViewModel.FirstOrDefault());
        }

        [HttpPost]
        public IActionResult Edit(UserDetailsViewModel userDetailsViewModel)
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<UserDetailsViewModel, User>()
                     .ForMember(pr => pr.Id, opt => opt.MapFrom(q => q.UserId))
                     .ForMember(pr => pr.LastName, opt => opt.MapFrom(q => q.LastName))
                     .ForMember(pr => pr.FirstName, opt => opt.MapFrom(q => q.FirstName))
                     .ForMember(pr => pr.Patronic, opt => opt.MapFrom(q => q.PatronicName));

                cfg.CreateMap<UserDetailsViewModel, UserInformation>()
                .ForMember(pr => pr.BirthDate, opt => opt.MapFrom(q => q.BirthDate))
                .ForMember(pr => pr.Email, opt => opt.MapFrom(q => q.Email))
                .ForMember(pr => pr.UserID, opt => opt.MapFrom(q => q.UserId))
                .ForMember(pr => pr.PhoneNum, opt => opt.MapFrom(q => q.PhoneNum));
            });

            var map = new Mapper(mapConfig);

            var user = map.Map<UserDetailsViewModel, User>(userDetailsViewModel);
            var userInformation = map.Map<UserDetailsViewModel, UserInformation>(userDetailsViewModel);

            _DB.User.Update(user);

            _DB.SaveChanges();
            _DB.UserInformation.Update(userInformation);
            _DB.SaveChanges();

            return RedirectToAction("Details", userDetailsViewModel.UserId);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}