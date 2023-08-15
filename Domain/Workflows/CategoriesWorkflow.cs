using Domain.DTOs.Categories;
using Domain.Helpers;
using Infrastructure.Contracts.UoW;
using Infrastructure.Entities;
using Infrastructure.Repositories;
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

        public async Task<ResultQuery<CategoryResultDTO>> Get(string name, int page, int pageSize)
        {
            var predicate = PredicateBuilder.New<Category>(true);

            if (!string.IsNullOrWhiteSpace(name))
                predicate = predicate.And(x => x.Name.Contains(name));

            var query = await _unitOfWork.Categories.Query(predicate, page, pageSize, "Name");

            return new ResultQuery<CategoryResultDTO>()
            {
                TotalRecords = query.TotalRecords,
                Results = query.Results.MapToDTO()
            };
        }

        public async Task Add(CategoryDTO categoryDTO)
        {
            var category = categoryDTO.MapToEntity();

            var categoryNameAlreadyExists = await _unitOfWork.Categories.Get(x => x.Name == categoryDTO.Name);

            if (categoryNameAlreadyExists.Any())
                AddError("Name", "Category name already exists", category.Name);

            if (IsValid)
            {
                await _unitOfWork.Categories.Add(category);

                await _unitOfWork.Commit();
            }
        }

        public async Task Update(int id, CategoryDTO categoryDTO)
        {
            var category = await _unitOfWork.Categories.Find(id);

            if (category == null)
            {
                AddError("Category", "Category not exists");
                return;
            }

            var categoryNameAlreadyExists = await _unitOfWork.Categories.Get(x => x.Id != id && x.Name == categoryDTO.Name);

            if (categoryNameAlreadyExists.Any())
                AddError("Name", "Category name already exists", category.Name);

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
                AddError("Category", "Category not exists");
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
    }
}
