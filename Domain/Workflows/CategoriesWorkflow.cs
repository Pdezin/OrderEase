using Domain.DTOs;
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

        public async Task<ResultQuery<CategoryDTO>> Get(string name, int page, int pageSize)
        {
            var predicate = PredicateBuilder.New<Category>(true);

            if (!string.IsNullOrWhiteSpace(name))
                predicate = predicate.And(x => x.Name.Contains(name));

            var query = await _unitOfWork.Categories.Query(predicate, page, pageSize);

            return new ResultQuery<CategoryDTO>()
            {
                TotalRecords = query.TotalRecords,
                Results = query.Results.MapToDTO()
            };
        }

        public async Task Add(CategoryDTO categoryDTO)
        {
            var category = categoryDTO.MapToEntity();

            var categoryExists = await _unitOfWork.Categories.Get(x => x.Name == categoryDTO.Name);

            if (categoryExists.Any())
                AddError("Name", "Category already exists", category.Name);

            if (IsValid)
            {
                await _unitOfWork.Categories.Add(category);

                await _unitOfWork.Commit();
            }
        }
    }
}
