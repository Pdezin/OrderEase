using Domain.DTOs.Categories;
using Domain.DTOs.PriceLists;
using Domain.Helpers;
using Domain.Workflows.Base;
using Infrastructure.Contracts.UoW;
using Infrastructure.Entities;
using Infrastructure.Repositories.Base;
using LinqKit;

namespace Domain.Workflows
{
    public class PriceListsWorkflow : WorkflowBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PriceListsWorkflow(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<QueryResult<PriceListsResultDTO>> Get(string name, bool? active, int page, int pageSize)
        {
            var predicate = PredicateBuilder.New<PriceList>(true);

            if (!string.IsNullOrWhiteSpace(name))
                predicate = predicate.And(x => x.Name.Contains(name));

            if (active != null)
                predicate = predicate.And(x => x.Active == active);

            var query = await _unitOfWork.PriceLists.Query(predicate, page, pageSize, "Name");

            return new QueryResult<PriceListsResultDTO>()
            {
                TotalRecords = query.TotalRecords,
                Results = Mapper.MapToListDTO<PriceListsResultDTO, PriceList>(query.Results)
            };
        }

        public async Task<PriceListsResultDTO?> Add(PriceListsDTO priceListsDTO)
        {
            await DataValidator(priceListsDTO);

            if (!IsValid)
                return null;

            var priceList = priceListsDTO.MapToEntity();

            await _unitOfWork.PriceLists.Add(priceList);

            await _unitOfWork.Commit();

            return priceList.MapToDTO();
        }

        public async Task Update(int id, PriceListsDTO priceListsDTO)
        {
            var priceList = await _unitOfWork.PriceLists.Find(id);

            if (priceList == null)
            {
                NotFound("Price List", "Price List not exists");
                return;
            }

            await DataValidator(priceListsDTO, id);

            if (IsValid)
            {
                priceList.Name = priceListsDTO.Name;
                priceList.Active = priceListsDTO.Active;

                _unitOfWork.PriceLists.Update(priceList);

                await _unitOfWork.Commit();
            }
        }

        private async Task DataValidator(PriceListsDTO priceListsDTO, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(priceListsDTO.Name))
                AddError("Name", "Name is required", priceListsDTO.Name);

            if (priceListsDTO.Name.Length > 100)
                AddError("Name", "Price List name cannot be longer than 100 characters", priceListsDTO.Name);

            var priceListsNameAlreadyExists = await _unitOfWork.PriceLists.Get(x => x.Id != id && x.Name == priceListsDTO.Name);

            if (priceListsNameAlreadyExists.Any())
                AddError("Name", "Price List name already exists", priceListsDTO.Name);
        }
    }
}
