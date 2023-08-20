using Domain.DTOs.Roles;
using Domain.Helpers;
using Domain.Workflows.Base;
using Infrastructure.Contracts.UoW;
using Infrastructure.Entities;
using Infrastructure.Repositories.Base;
using LinqKit;

namespace Domain.Workflows
{
    public class RolesWorkflow : WorkflowBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolesWorkflow(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<QueryResult<RoleResultDTO>> Get(string name, int page, int pageSize)
        {
            var predicate = PredicateBuilder.New<Role>(true);

            if (!string.IsNullOrWhiteSpace(name))
                predicate = predicate.And(x => x.Name.Contains(name));

            var query = await _unitOfWork.Roles.Query(predicate, page, pageSize, "Name");

            var teste = Mapper.MapToListDTO<RoleResultDTO, Role>(query.Results);

            return new QueryResult<RoleResultDTO>()
            {
                TotalRecords = query.TotalRecords,
                Results = teste
            };
        }

        public async Task<RoleResultDTO?> Add(RoleDTO roleDTO)
        {
            await DataValidator(roleDTO);

            if (!IsValid)
                return null;

            var role = roleDTO.MapToEntity();

            await _unitOfWork.Roles.Add(role);

            await _unitOfWork.Commit();

            return role.MapToDTO();
        }

        public async Task Update(int id, RoleDTO roleDTO)
        {
            var role = await _unitOfWork.Roles.Find(id);

            if (role == null)
            {
                NotFound("Role", "Role not exists");
                return;
            }

            await DataValidator(roleDTO, id);

            if (IsValid)
            {
                role.Name = roleDTO.Name;
                role.UserAccess = roleDTO.UserAccess;
                role.ProductAccess = roleDTO.ProductAccess;
                role.OrderAccess = roleDTO.OrderAccess;

                _unitOfWork.Roles.Update(role);

                await _unitOfWork.Commit();
            }
        }

        public async Task Delete(int id)
        {
            var role = await _unitOfWork.Roles.Find(id);

            if (role == null)
            {
                NotFound("Role", "Role not exists");
                return;
            }

            if (role.Users.Count > 0)
                AddError("Role", "There are users using this role");

            if (IsValid)
            {
                _unitOfWork.Roles.Remove(role);

                await _unitOfWork.Commit();
            }
        }

        private async Task DataValidator(RoleDTO roleDTO, int id = 0)
        {
            if (roleDTO.UserAccess <= 0)
                AddError("UserAccess", "User Access is required", roleDTO.UserAccess);

            if (roleDTO.ProductAccess <= 0)
                AddError("ProductAccess", "Product Access is required", roleDTO.ProductAccess);

            if (roleDTO.OrderAccess <= 0)
                AddError("OrderAccess", "Order Access is required", roleDTO.OrderAccess);

            if (string.IsNullOrWhiteSpace(roleDTO.Name))
                AddError("Name", "Name is required", roleDTO.Name);

            if (roleDTO.Name.Length > 100)
                AddError("Name", "Role name cannot be longer than 100 characters", roleDTO.Name);

            var roleNameAlreadyExists = await _unitOfWork.Roles.Get(x => x.Id != id && x.Name == roleDTO.Name);

            if (roleNameAlreadyExists.Any())
                AddError("Name", "Role name already exists", roleDTO.Name);
        }
    }
}
