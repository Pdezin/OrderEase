using Domain.DTOs.Products;
using Domain.Helpers;
using Domain.Workflows.Base;
using Infrastructure.Contracts.UoW;
using Infrastructure.Entities;
using Infrastructure.Repositories.Base;
using Infrastructure.UoW;
using LinqKit;
using System.Linq;

namespace Domain.Workflows
{
    public class ProductsWorkflow : WorkflowBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsWorkflow(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<QueryResult<ProductResultDTO>> Get(string term, bool? active, int page, int pageSize, string orderBy, bool orderDesc)
        {
            var predicate = PredicateBuilder.New<Product>(true);

            if (!string.IsNullOrWhiteSpace(term))
            {
                var predicateTerm = PredicateBuilder.New<Product>(false);

                if (term.ToInt() > 0)
                    predicateTerm = predicateTerm.Or(x => x.Id == term.ToInt());

                predicateTerm = predicateTerm.Or(x => x.Name.Contains(term));
                predicateTerm = predicateTerm.Or(x => x.Description.Contains(term));
                predicateTerm = predicateTerm.Or(x => x.Category.Name.Contains(term));

                predicate = predicate.And(predicateTerm);
            }

            if (active != null)
                predicate = predicate.And(x => x.Active == active);

            var query = await _unitOfWork.Products.Query(predicate, $"{nameof(Product.Category)}, {nameof(Product.ProductPrices)}", page, pageSize, orderBy, orderDesc);

            return new QueryResult<ProductResultDTO>()
            {
                TotalRecords = query.TotalRecords,
                Results = Mapper.MapToListDTO<ProductResultDTO, Product>(query.Results)
            };
        }

        public async Task<ProductResultDetailDTO?> Detail(int id)
        {
            var product = await _unitOfWork.Products.Find(x => x.Id == id, $"{nameof(Product.Category)}, {nameof(Product.ProductPrices)}");

            if (product == null)
            {
                NotFound("Product", "Product not exists");
                return null;
            }

            return product.MapToDetailDTO();
        }

        public async Task<ProductResultDetailDTO?> Add(ProductDTO productDTO)
        {
            await DataValidator(productDTO);

            var category = await _unitOfWork.Categories.Find(productDTO.CategoryId);

            if (category == null)
                AddError("CategoryId", "Category is not exists", productDTO.CategoryId);

            if (productDTO.ProductPrices != null)
                productDTO.ProductPrices.RemoveAll(x => x.Remove == true);

            if (!IsValid)
                return null;

            var product = productDTO.MapToEntity();

            if (productDTO.ProductPrices != null)
                foreach (var price in productDTO.ProductPrices.Select(x => x.MapToEntity()))
                    product.ProductPrices.Add(price);

            product.Category = category;

            await _unitOfWork.Products.Add(product);

            await _unitOfWork.Commit();

            return product.MapToDetailDTO();
        }

        public async Task Update(int id, ProductDTO productDTO)
        {
            var product = await _unitOfWork.Products.Find(x => x.Id == id, $"{nameof(Product.Category)}, {nameof(Product.ProductPrices)}");

            if (product == null)
            {
                NotFound("Product", "Product not exists");
                return;
            }

            await DataValidator(productDTO, id);

            var category = await _unitOfWork.Categories.Find(productDTO.CategoryId);

            if (category == null)
                AddError("CategoryId", "Category is not exists", productDTO.CategoryId);
            
            if (IsValid)
            {
                product.Name = productDTO.Name;
                product.Description = productDTO.Description;
                product.Unit = productDTO.Unit;
                product.Stock = productDTO.Stock;
                product.Width = productDTO.Width;
                product.Height = productDTO.Height;
                product.Length = productDTO.Length;
                product.Weight = productDTO.Weight;
                product.Active = productDTO.Active;

                product.Category = category;

                if (productDTO.ProductPrices != null)
                {
                    foreach (var priceDTO in productDTO.ProductPrices)
                    {
                        var price = product.ProductPrices.FirstOrDefault(x => x.PriceListId == priceDTO.PriceListId);

                        if (price == null)
                        {
                            product.ProductPrices.Add(priceDTO.MapToEntity());
                            continue;
                        }

                        if (priceDTO.Remove == true)
                            _unitOfWork.Context.ProductPrices.Remove(price);
                        else
                            price.Price = priceDTO.Price;
                    }
                }

                _unitOfWork.Products.Update(product);

                await _unitOfWork.Commit();
            }
        }

        private async Task DataValidator(ProductDTO productDTO, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(productDTO.Name))
                AddError("Name", "Name is required", productDTO.Name);

            if (productDTO.Name.Length > 200)
                AddError("Name", "User name cannot be longer than 200 characters", productDTO.Name);

            if (string.IsNullOrWhiteSpace(productDTO.Description))
                AddError("Description", "Description is required", productDTO.Description);

            if (productDTO.Description.Length > 2000)
                AddError("Description", "Product description cannot be longer than 2000 characters", productDTO.Description);

            if (string.IsNullOrWhiteSpace(productDTO.Unit))
                AddError("Unit", "Unit is required", productDTO.Unit);

            if (productDTO.Unit.Length > 3)
                AddError("Unit", "Product unit cannot be longer than 3 characters");

            if (productDTO.CategoryId <= 0)
                AddError("CategoryId", "Category is required", productDTO.CategoryId);

            var productAlreadyExists = await _unitOfWork.Products.Get(x => x.Id != id && x.Name == productDTO.Name);

            if (productAlreadyExists.Any())
                AddError("Name", "Product name already exists", productDTO.Name);

            if (productDTO.ProductPrices != null && productDTO.ProductPrices.Count > 0)
            {
                if (productDTO.ProductPrices.GroupBy(x => x.PriceListId).Any(g => g.Count() > 1))
                    AddError("ProductPrices", "Duplicate PriceListId");

                var productPrices = await _unitOfWork.PriceLists.Get(x => productDTO.ProductPrices.Select(y => y.PriceListId).Contains(x.Id));

                var productPricesNotExists = productDTO.ProductPrices.Where(x => !productPrices.Select(x => x.Id).Contains(x.PriceListId));

                if (productPricesNotExists.Any())
                    AddError("ProductPrices", $"PriceListId [{ string.Join(", ", productPricesNotExists.Select(x => x.PriceListId)) }] is not exists");
            }
        }
    }
}
