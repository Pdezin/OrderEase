using Domain.DTOs.Categories;
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

        #endregion
    }
}
