using Domain.DTOs.Categories;
using Domain.DTOs.Roles;
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

        #endregion

        #region To DTO

        public static CategoryResultDTO MapToDTO(this Category m)
        {
            return new CategoryResultDTO()
            {
                Id = m.Id,
                Name = m.Name,
                CreatedAt = m.CreatedAt
            };
        }

        public static IEnumerable<CategoryResultDTO> MapToDTO(this IEnumerable<Category> m)
        {
            return m.Select(x => x.MapToDTO());
        }

        public static RoleResultDTO MapToDTO(this Role m)
        {
            return new RoleResultDTO()
            {
                Id = m.Id,
                Name = m.Name,
                OrderAccess = m.OrderAccess,
                ProductAccess = m.ProductAccess,
                UserAccess = m.UserAccess,
                CreatedAt = m.CreatedAt
            };
        }

        public static IEnumerable<RoleResultDTO> MapToDTO(this IEnumerable<Role> m)
        {
            return m.Select(x => x.MapToDTO());
        }

        #endregion
    }
}
