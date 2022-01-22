using Service.DTO;

namespace BusinessLayer.Service
{
    public interface IDTOService
    {
        void CreateUser(UserDTO userDTO);
        int CreateCheck(CheckDTO checkDTO);
        void Delete(int id);
        IEnumerable<UserDTO> GetUsers();
        UserInformationDTO GetDetail(int userID);
        int EditSave(UserInformationDTO userInformationDTO);

        CheckDTO GetCheck(int id);

        IEnumerable<CheckDTO> GetChecks(int userID);

        void Dispose();
    }
}