using Domain.DTOs.Categories;
using Domain.DTOs.PriceLists;
using Domain.DTOs.Products;
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

        public static Product MapToEntity(this ProductDTO m)
        {
            return new Product()
            {
                Name = m.Name,
                Description = m.Description,
                Unit = m.Unit,
                Stock = m.Stock,
                Width = m.Width,
                Height = m.Height,
                Length = m.Length,
                Weight = m.Weight,
                CategoryId = m.CategoryId,
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

        public static ProductResultDTO MapToDTO(this Product m)
        {
            return new ProductResultDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Stock = m.Stock,
                Unit = m.Unit,
                Category = m.Category.Name,
                CategoryId = m.CategoryId,
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
                Role = m.Role.Name,
                BirthDate = m.BirthDate?.ToStringDateTime() ?? "",
                CreatedAt = m.CreatedAt.ToStringDateTime(),
                UpdatedAt = m.UpdatedAt.ToStringDateTime(),
                Active = m.Active
            };
        }

        public static ProductResultDetailDTO MapToDetailDTO(this Product m)
        {
            return new ProductResultDetailDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Stock = m.Stock,
                Unit = m.Unit,
                Height = m.Height,
                Length = m.Length,
                Weight = m.Weight,
                Width = m.Width,
                Category = m.Category.Name,
                CategoryId = m.CategoryId,
                CreatedAt = m.CreatedAt.ToStringDateTime(),
                UpdatedAt = m.UpdatedAt.ToStringDateTime(),
                Active = m.Active
            };
        }

        #endregion
    }
}
