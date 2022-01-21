using DataLayer;
using ShopManagerSystems.ViewModel;

namespace ShopManagerSystems.Service
{
    public class DTOService
    {
        private readonly ApplicationDBContext _DB;

        public DTOService(ApplicationDBContext dBContex)
        {
            _DB = dBContex;
        }
        public UserDetailsViewModel GetDetailsViewModel(int userID)
        {
            var userDetailsViewModel = from u in _DB.User
                                       join ui in _DB.UserInformation
                                       on u.Id equals ui.UserID

                                       into UserInfo
                                       from item in UserInfo.DefaultIfEmpty()
                                       where u.Id == userID
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
            return userDetailsViewModel.FirstOrDefault();
        }
    }
}
