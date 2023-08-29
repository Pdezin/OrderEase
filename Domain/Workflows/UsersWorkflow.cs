using Domain.DTOs.Use;
using Domain.DTOs.Users;
using Domain.Helpers;
using Domain.Workflows.Base;
using Infrastructure.Contracts.UoW;
using Infrastructure.Entities;
using Infrastructure.Repositories.Base;
using LinqKit;

namespace Domain.Workflows
{
    public class UsersWorkflow : WorkflowBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersWorkflow(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<QueryResult<UserResultDTO>> Get(string term, bool? active, int page, int pageSize)
        {
            var predicate = PredicateBuilder.New<User>(true);

            if (!string.IsNullOrWhiteSpace(term))
            {
                var predicateTerm = PredicateBuilder.New<User>(false);

                if (term.ToInt() > 0)
                    predicateTerm = predicateTerm.Or(x => x.Id == term.ToInt());

                predicateTerm = predicateTerm.Or(x => x.Name.Contains(term));
                predicateTerm = predicateTerm.Or(x => x.Email.Contains(term));
                predicateTerm = predicateTerm.Or(x => x.Role.Name.Contains(term));

                predicate = predicate.And(predicateTerm);
            }

            if (active != null)
                predicate = predicate.And(x => x.Active == active);

            var query = await _unitOfWork.Users.Query(predicate, nameof(User.Role), page, pageSize, nameof(User.Name));

            return new QueryResult<UserResultDTO>()
            {
                TotalRecords = query.TotalRecords,
                Results = Mapper.MapToListDTO<UserResultDTO, User>(query.Results)
            };
        }

        public async Task<UserResultDetailDTO?> Detail(int id)
        {
            var user = await _unitOfWork.Users.Find(x => x.Id == id, nameof(User.Role));

            if (user == null)
            {
                NotFound("User", "User not exists");
                return null;
            }

            return user.MapToDetailDTO();
        }

        public async Task<UserResultDetailDTO?> Add(UserDTO userDTO)
        {
            await DataValidator(userDTO);

            var role = await _unitOfWork.Roles.Find(userDTO.RoleId);
            
            if (role == null)
                AddError("RoleId", "Role is not exists", userDTO.RoleId);

            if (!IsValid)
                return null;

            var user = userDTO.MapToEntity();

            user.Role = role;

            await _unitOfWork.Users.Add(user);

            await _unitOfWork.Commit();

            return user.MapToDetailDTO();
        }

        public async Task Update(int id, UserDTO userDTO)
        {
            var user = await _unitOfWork.Users.Find(id);

            if (user == null)
            {
                NotFound("User", "User not exists");
                return;
            }

            await DataValidator(userDTO);

            var role = await _unitOfWork.Roles.Find(userDTO.RoleId);

            if (role == null)
                AddError("RoleId", "Role is not exists", userDTO.RoleId);

            if (IsValid)
            {
                user.Name = userDTO.Name;
                user.Email = userDTO.Email;
                user.Password = userDTO.Password;
                user.BirthDate = userDTO.BirthDate.ToDateTimeUTC();
                user.RoleId = userDTO.RoleId;
                user.Active = userDTO.Active;

                user.Role = role;

                _unitOfWork.Users.Update(user);

                await _unitOfWork.Commit();
            }
        }

        private async Task DataValidator(UserDTO userDTO, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Name))
                AddError("Name", "Name is required", userDTO.Name);
            
            if (userDTO.Name.Length > 200)
                AddError("Name", "User name cannot be longer than 200 characters", userDTO.Name);

            if (string.IsNullOrWhiteSpace(userDTO.Email))
                AddError("Email", "E-mail is required", userDTO.Email);

            if (userDTO.Email.Length > 50)
                AddError("Email", "User e-mail cannot be longer than 50 characters", userDTO.Email);

            if (string.IsNullOrWhiteSpace(userDTO.Password))
                AddError("Password", "Password is required", userDTO.Password);

            if (userDTO.Password.Length > 20)
                AddError("Password", "User password cannot be longer than 20 characters");

            if (userDTO.Password.Length < 8)
                AddError("Password", "User password must be at least 8 characters long");

            var isValidDateFormat = string.IsNullOrWhiteSpace(userDTO.BirthDate) || userDTO.BirthDate.ToDateTimeUTC() != null;

            if (!isValidDateFormat)
                AddError("BirthDate", "User birth date invalid. Correct format yyyy-MM-dd", userDTO.BirthDate);

            if (userDTO.RoleId <= 0)
                AddError("RoleId", "Role is required", userDTO.RoleId);

            var userAlreadyExists = await _unitOfWork.Users.Get(x => x.Id != id && x.Email == userDTO.Email);

            if (userAlreadyExists.Any())
                AddError("Email", "User e-mail already exists", userDTO.Email);
        }
    }
}
