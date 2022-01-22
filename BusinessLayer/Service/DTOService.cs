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

        public void CreateUser(UserDTO userDTO)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();

            var user = mapper.Map<UserDTO, User>(userDTO);

            _DB.User.Add(user);
            _DB.SaveChanges();

        }

        public void Dispose()
        {
            _DB.Dispose();
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

            return  mapper.Map<IEnumerable<User>,List<UserDTO>>(users);
        }
    }
}
