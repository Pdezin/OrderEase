using Domain.DTOs.Categories;
using Domain.DTOs.PriceLists;
using Domain.DTOs.Roles;
using Domain.DTOs.Use;
using Domain.DTOs.Users;
using Infrastructure.Entities;

namespace Domain.Helpers
{
    public static class Mapper
    {
        #region To Entity

        public static Category MapToEntity(this CategoryDTO m)
        {
            return new Category()
            {
                Name = m.Name
            };
        }

        public static Role MapToEntity(this RoleDTO m)
        {
            return new Role()
            {
                Name = m.Name,
                OrderAccess = m.OrderAccess,
                ProductAccess = m.ProductAccess,
                UserAccess = m.UserAccess
            };
        }

        public static PriceList MapToEntity(this PriceListsDTO m)
        {
            return new PriceList()
            {
                Name = m.Name,
                Active = m.Active
            };
        }

        public static User MapToEntity(this UserDTO m)
        {
            return new User()
            {
                Name = m.Name,
                Email = m.Email,
                Password = m.Password,
                RoleId = m.RoleId,
                BirthDate = m.BirthDate.ToDateTimeUTC(),
                Active = m.Active
            };
        }

        #endregion

        #region To DTO

        public static CategoryResultDTO MapToDTO(this Category m)
        {
            return new CategoryResultDTO()
            {
                Id = m.Id,
                Name = m.Name
            };
        }

        public static RoleResultDTO MapToDTO(this Role m)
        {
            return new RoleResultDTO()
            {
                Id = m.Id,
                Name = m.Name,
                OrderAccess = m.OrderAccess,
                ProductAccess = m.ProductAccess,
                UserAccess = m.UserAccess
            };
        }

        public static PriceListsResultDTO MapToDTO(this PriceList m)
        {
            return new PriceListsResultDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Active = m.Active
            };
        }

        public static UserResultDTO MapToDTO(this User m)
        {
            return new UserResultDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Active = m.Active
            };
        }

        public static IEnumerable<DTO> MapToListDTO<DTO, ENTITY>(IEnumerable<ENTITY> m)
        {
            var parameterTypes = new Type[] { typeof(ENTITY) };
            var staticMethod = typeof(Mapper).GetMethod("MapToDTO", parameterTypes);

            if (staticMethod != null)
                return m.Select(x => (DTO)staticMethod.Invoke(null, new object[] { x }));

            return Enumerable.Empty<DTO>();
        }

        #endregion

        #region To Detail DTO

        public static UserResultDetailDTO MapToDetailDTO(this User m)
        {
            return new UserResultDetailDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                RoleId = m.RoleId,
                BirthDate = m.BirthDate?.ToStringDateTime() ?? "",
                CreatedAt = m.CreatedAt.ToStringDateTime(),
                UpdatedAt = m.UpdatedAt.ToStringDateTime(),
                Active = m.Active
            };
        }

        #endregion
    }
}
