using DataLayer;
using Service.DTO;
using AutoMapper;
using DataLayer.DataLayer;

namespace BusinessLayer.Service
{
    public class DTOService : IDTOService
    {
        private readonly ApplicationDBContext _DB;

        public DTOService(ApplicationDBContext dBContex)
        {
            _DB = dBContex;
        }

        public int CreateCheck(CheckDTO checkDTO)
        {
            var map = new MapperConfiguration(cfg => cfg.CreateMap<CheckDTO, Check>()).CreateMapper();

            var check = map.Map<CheckDTO, Check>(checkDTO);

            _DB.Check.Add(check);

            return check.Id;

        }

        public void CreateUser(UserDTO userDTO)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();

            var user = mapper.Map<UserDTO, User>(userDTO);

            _DB.User.Add(user);
            _DB.SaveChanges();

        }

        public void Delete(int id)
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
        }

        public void Dispose()
        {
            _DB.Dispose();
        }

        public int EditSave(UserInformationDTO userInformationDTO)
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserInformationDTO, User>()
                     .ForMember(pr => pr.Id, opt => opt.MapFrom(q => q.UserId))
                     .ForMember(pr => pr.LastName, opt => opt.MapFrom(q => q.LastName))
                     .ForMember(pr => pr.FirstName, opt => opt.MapFrom(q => q.FirstName))
                     .ForMember(pr => pr.PatronicName, opt => opt.MapFrom(q => q.PatronicName));

                cfg.CreateMap<UserInformationDTO, UserInformation>()
                .ForMember(pr => pr.Id, opt => opt.MapFrom(q => q.UIID))
                .ForMember(pr => pr.BirthDate, opt => opt.MapFrom(q => q.BirthDate))
                .ForMember(pr => pr.Email, opt => opt.MapFrom(q => q.Email))
                .ForMember(pr => pr.UserID, opt => opt.MapFrom(q => q.UserId))
                .ForMember(pr => pr.PhoneNum, opt => opt.MapFrom(q => q.PhoneNum));
            });

            var map = new Mapper(mapConfig);
            var user = map.Map<UserInformationDTO, User>(userInformationDTO);
            var userInformation = map.Map<UserInformationDTO, UserInformation>(userInformationDTO);

            _DB.User.Update(user);
            _DB.SaveChanges();
            if (userInformation.Id == 0)
            {
                _DB.UserInformation.Add(userInformation);
            }
            else
            {
                _DB.UserInformation.Update(userInformation);
            }
            _DB.SaveChanges();

            return user.Id;

        }

        public CheckDTO GetCheck(int id)
        {
            var check = _DB.Check.Find(id);
            var map = new MapperConfiguration(cfg => cfg.CreateMap<Check, CheckDTO>()).CreateMapper();
            var checkDTO = map.Map<Check, CheckDTO>(check);
            return checkDTO;
        }

        public IEnumerable<CheckDTO> GetChecks(int userID)
        {
            var Checks = _DB.Check.ToList();

            var map = new MapperConfiguration(cfg => cfg.CreateMap<Check, CheckDTO>()).CreateMapper();

            var ChecksDTO = map.Map<IEnumerable<Check>, List<CheckDTO>>(Checks);

            return ChecksDTO;



        }

        public UserInformationDTO GetDetail(int userID)
        {
            var userDetailsViewModel = from u in _DB.User
                                       join ui in _DB.UserInformation
                                       on u.Id equals ui.UserID

                                       into UserInfo
                                       from item in UserInfo.DefaultIfEmpty()
                                       where u.Id == userID
                                       select new UserInformationDTO
                                       {
                                           UserId = u.Id,
                                           UIID = item == null ? 0 : item.Id,
                                           LastName = u.LastName,
                                           FirstName = u.FirstName,
                                           PatronicName = u.PatronicName,
                                           BirthDate = item.BirthDate,
                                           Email = item.Email,
                                           PhoneNum = item.PhoneNum
                                       };
            return userDetailsViewModel.FirstOrDefault();
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var users = _DB.User.ToList();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(users);
        }
    }
}
