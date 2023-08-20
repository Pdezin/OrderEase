using Domain.DTOs.Categories;
using Domain.DTOs.PriceLists;
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

        public static PriceList MapToEntity(this PriceListsDTO m)
        {
            return new PriceList()
            {
                Name = m.Name,
                Active = m.Active,
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

        public static PriceListsResultDTO MapToDTO(this PriceList m)
        {
            return new PriceListsResultDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Active = m.Active,
                CreatedAt = m.CreatedAt
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
    }
}
