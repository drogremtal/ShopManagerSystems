using Service.DTO;

namespace BusinessLayer.Service
{
    public interface IDTOService
    {
        void CreateUser(UserDTO userDTO);
        IEnumerable<UserDTO> GetUsers();
        UserInformationDTO GetDetail(int userID);
        void Dispose();
    }
}