using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.APIServices.IServiceBackendAPIs
{
    public interface ISystemUserBackendAPI
    {
        //Đăng nhập.
        Task<ResultMessage<string>> Authenticate(LoginRequest request);
        Task<ResultMessage<TokenRequest>> AuthenticateWithTwoToken(LoginRequest request);
        //DS User
        Task<ResultMessage<PageResultModel<SystemUserModel>>> GetUserPaging(UserPagingRequest request);
        //Tạo User
        Task<ResultMessage<bool>> Signup(SignUpRequest request);
        //Sửa User
        Task<ResultMessage<bool>> Update(Guid ID, UserUpdateRequest request);
        //Get User theo ID.
        Task<ResultMessage<SystemUserModel>> GetUserByID(Guid ID);
        //Xóa User
        Task<ResultMessage<bool>> Delete(Guid ID);
        //Thêm quyền và xóa quyền theo UserID
        Task<ResultMessage<bool>> RoleAssign(Guid ID, RoleAssignRequest request);



    }
}
