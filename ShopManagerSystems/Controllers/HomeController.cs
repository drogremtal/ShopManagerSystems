using Microsoft.AspNetCore.Mvc;
using ShopManagerSystems.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using DataLayer.DataLayer;
using AutoMapper;
using ShopManagerSystems.ViewModel;

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
            var user = _DB.User.Find(id);
            var userDetails = _DB.UserInformation.Find(user.Id);

            var config = new MapperConfiguration(cfg=>{
                cfg.CreateMap<User, UserDetailsViewModel>()
               .ForMember(pr => pr.UserId, opt => opt.MapFrom(q => q.Id))
               .ForMember(pr => pr.LastName, opt => opt.MapFrom(q => q.LastName))
               .ForMember(pr => pr.FirstName, opt => opt.MapFrom(q => q.FirstName))
               .ForMember(pr => pr.PatronicName, opt => opt.MapFrom(q => q.Patronic));
                cfg.CreateMap<UserInformation, UserDetailsViewModel>()
                .ForMember(pr => pr.BirthDate, opt => opt.MapFrom(q => q.BirthDate))
                .ForMember(pr => pr.Email, opt => opt.MapFrom(q => q.Email))
                .ForMember(pr => pr.PhoneNum, opt => opt.MapFrom(q => q.PhoneNum));
            }) ;


            var map = new Mapper(config);


            var userDetailsViewModel = map.Map<User, UserDetailsViewModel>(user);
            map.Map(userDetails, userDetailsViewModel);
            
            return View(userDetailsViewModel);
        }

        public IActionResult Edit(int id)
        {
            var user = _DB.User.Find(id);
            var userDetails = _DB.UserInformation.Find(user.Id);

            var config = new MapperConfiguration(cfg => {
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
            map.Map(userDetails, userDetailsViewModel);

            return View(userDetailsViewModel);
        }

        [HttpPost]
        public IActionResult Edit(UserDetailsViewModel userDetailsViewModel)
        {
            var mapConfig = new MapperConfiguration(cfg=>{
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
            _DB.UserInformation.Update(userInformation);
            _DB.SaveChanges();
            return RedirectToAction("Details",userDetailsViewModel.UserId);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}