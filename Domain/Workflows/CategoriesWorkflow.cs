using Domain.DTOs.Categories;
using Domain.Helpers;
using Domain.Workflows.Base;
using Infrastructure.Contracts.UoW;
using Infrastructure.Entities;
using Infrastructure.Repositories.Base;
using LinqKit;

namespace Domain.Workflows
{
    public class CategoriesWorkflow : WorkflowBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesWorkflow(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<QueryResult<CategoryResultDTO>> Get(string term, int page, int pageSize)
        {
            var predicate = PredicateBuilder.New<Category>(true);

            if (!string.IsNullOrWhiteSpace(term))
                predicate = predicate.And(x => x.Name.Contains(term));

            var query = await _unitOfWork.Categories.Query(predicate, page, pageSize, nameof(Category.Name));

            return new QueryResult<CategoryResultDTO>()
            {
                TotalRecords = query.TotalRecords,
                Results = Mapper.MapToListDTO<CategoryResultDTO, Category>(query.Results)
            };
        }

        public async Task<CategoryResultDTO?> Add(CategoryDTO categoryDTO)
        {
            await DataValidator(categoryDTO);

            if (!IsValid)
                return null;

            var category = categoryDTO.MapToEntity();

            await _unitOfWork.Categories.Add(category);

            await _unitOfWork.Commit();

            return category.MapToDTO();
        }

        public async Task Update(int id, CategoryDTO categoryDTO)
        {
            var category = await _unitOfWork.Categories.Find(id);

            if (category == null)
            {
                NotFound("Category", "Category not exists");
                return;
            }

            await DataValidator(categoryDTO);

            if (IsValid)
            {
                category.Name = categoryDTO.Name;

                _unitOfWork.Categories.Update(category);

                await _unitOfWork.Commit();
            }
        }

        public async Task Delete(int id)
        {
            var category = await _unitOfWork.Categories.Find(id);

            if (category == null)
            {
                NotFound("Category", "Category not exists");
                return;
            }

            if (category.Products.Count > 0)
                AddError("Category", "There are products using this category");
            
            if (IsValid)
            {
                _unitOfWork.Categories.Remove(category);

                await _unitOfWork.Commit();
            }
        }

        private async Task DataValidator(CategoryDTO categoryDTO, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(categoryDTO.Name))
                AddError("Name", "Name is required", categoryDTO.Name);

            if (categoryDTO.Name.Length > 100)
                AddError("Name", "Category name cannot be longer than 100 characters", categoryDTO.Name);

            var categoryNameAlreadyExists = await _unitOfWork.Categories.Get(x => x.Id != id && x.Name == categoryDTO.Name);

            if (categoryNameAlreadyExists.Any())
                AddError("Name", "Category name already exists", categoryDTO.Name);
        }
    }
}
