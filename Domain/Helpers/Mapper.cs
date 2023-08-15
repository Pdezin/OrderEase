using Domain.DTOs;
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
                Id = m.Id,
                Name = m.Name
            };
        }

        #endregion

        #region To DTO

        public static CategoryDTO MapToDTO(this Category m)
        {
            return new CategoryDTO()
            {
                Id = m.Id,
                Name = m.Name,
                CreatedAt = m.CreatedAt
            };
        }

        public static IEnumerable<CategoryDTO> MapToDTO(this IEnumerable<Category> m)
        {
            return m.Select(x => x.MapToDTO());
        }

        #endregion
    }
}
