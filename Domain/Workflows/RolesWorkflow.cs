using Domain.DTOs.Roles;
using Domain.Helpers;
using Infrastructure.Contracts.UoW;
using Infrastructure.Entities;
using Infrastructure.Repositories;
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

        public async Task<ResultQuery<RoleResultDTO>> Get(string name, int page, int pageSize)
        {
            var predicate = PredicateBuilder.New<Role>(true);

            if (!string.IsNullOrWhiteSpace(name))
                predicate = predicate.And(x => x.Name.Contains(name));

            var query = await _unitOfWork.Roles.Query(predicate, page, pageSize, "Name");

            return new ResultQuery<RoleResultDTO>()
            {
                TotalRecords = query.TotalRecords,
                Results = query.Results.MapToDTO()
            };
        }

        public async Task Add(RoleDTO roleDTO)
        {
            if (roleDTO.Name.Length > 100)
                AddError("Name", "Role name cannot be longer than 100 characters", roleDTO.Name);

            var roleNameAlreadyExists = await _unitOfWork.Roles.Get(x => x.Name == roleDTO.Name);

            if (roleNameAlreadyExists.Any())
                AddError("Name", "Role name already exists", roleDTO.Name);

            if (IsValid)
            {
                var role = roleDTO.MapToEntity();

                await _unitOfWork.Roles.Add(role);

                await _unitOfWork.Commit();
            }
        }

        public async Task Update(int id, RoleDTO roleDTO)
        {
            var role = await _unitOfWork.Roles.Find(id);

            if (role == null)
            {
                AddError("Role", "Role not exists");
                return;
            }

            if (roleDTO.Name.Length > 100)
                AddError("Name", "Role name cannot be longer than 100 characters", roleDTO.Name);

            var roleNameAlreadyExists = await _unitOfWork.Roles.Get(x => x.Id != id && x.Name == roleDTO.Name);

            if (roleNameAlreadyExists.Any())
                AddError("Name", "Role name already exists", roleDTO.Name);

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
                AddError("Role", "Role not exists");
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
    }
}
