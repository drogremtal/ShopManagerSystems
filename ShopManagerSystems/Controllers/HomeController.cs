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
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserCreateViewModel, UserDTO>()).CreateMapper();

            var userDto = mapper.Map<UserCreateViewModel, UserDTO>(user);
            _Service.CreateUser(userDto);

            return RedirectToAction("UserList");
        }

        public IActionResult Details(int id)
        {
            var userInfo = _Service.GetDetail(id);
            var mapping = new MapperConfiguration(cfg => cfg.CreateMap<UserInformationDTO, UserInformationViewModel>()).CreateMapper();
            var userInfoView = mapping.Map<UserInformationDTO, UserInformationViewModel>(userInfo);
            return View(userInfoView);
        }

        public IActionResult Edit(int id)
        {
            var userEdit = _Service.GetDetail(id);

            var mapping = new MapperConfiguration(cfg => cfg.CreateMap<UserInformationDTO, UserInformationEditViewModel>()).CreateMapper();
            var userInfoView = mapping.Map<UserInformationDTO, UserInformationEditViewModel>(userEdit);

            if (userInfoView == null)
            {
                return View("UserList");
            }

            return View(userInfoView);
        }

        [HttpPost]
        public IActionResult Edit(UserInformationEditViewModel userDetailsViewModel)
        {
            var map = new MapperConfiguration(cfg => cfg.CreateMap<UserInformationEditViewModel, UserInformationDTO>()).CreateMapper();

            var UserInfoDTO = map.Map<UserInformationEditViewModel, UserInformationDTO>(userDetailsViewModel);

            int id = _Service.EditSave(UserInfoDTO);

            return RedirectToAction("Details", new { id = id });
        }

        public IActionResult Delete(int id)
        {
            _Service.Delete(id);
            return RedirectToAction("UserList");
        }

        [HttpPost]
        public IActionResult CreateCheck(CheckViewModel check)
        {
            if (check.FileLink !=null)
            {
                string path = Path.Combine("Upload", check.Id.ToString(), check.FileLink.FileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    check.FileLink.CopyTo(filestream);
                }
         
            }


            var map = new MapperConfiguration(cfg => cfg.CreateMap<CheckViewModel, CheckDTO>()
            .ForMember(q=>q.FileLink,src=>src.MapFrom(t=>t.FileLink.FileName))
            ).CreateMapper();

            var checkDto = map.Map<CheckViewModel, CheckDTO>(check);

            int id = _Service.CreateCheck(checkDto);

            return RedirectToAction("Check", new { id = id });
        }

        public IActionResult CreateCheck(int id)
        {
            return View(new CheckViewModel() { UserId = id });
        }

        public IActionResult Check(int id)
        {
            var checkDTO = _Service.GetCheck(id);

            var map = new MapperConfiguration(cfg => cfg.CreateMap<CheckDTO, CheckViewModel>()).CreateMapper();

            var check = map.Map<CheckDTO, CheckViewModel>(checkDTO);

            return View(check);
        }

        public IActionResult CheckList(int id)
        {
            var ChecksDTO = _Service.GetChecks(id);

            var map = new MapperConfiguration(cfg => cfg.CreateMap<CheckDTO, CheckViewModel>().
            ForMember(pr=>pr.FileLink.FileName,src=>src.MapFrom(q=>Path.Combine("Upload",id.ToString(), q.FileLink)))).
            CreateMapper();

            var CheckList = map.Map<IEnumerable<CheckDTO>, IEnumerable<CheckViewModel>>(ChecksDTO);

            ViewBag.UserId = id;

            return View(CheckList);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




    }
}