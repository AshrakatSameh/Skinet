using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        // private readonly IProductRepository _repo;     // Add this line
        // public ProductController(IProductRepository repo)     // Add this constructor
        // {
        //     _repo = repo;
        // }
        private readonly IGenericRepository<Product> _ProductRepo;
        private readonly IGenericRepository<ProductType> _ProductTypeRepo;
        private readonly IGenericRepository<ProductBrand> _ProductBrandRepo;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product> productRepo, 
        IGenericRepository<ProductType> productTypeRepo, 
        IGenericRepository<ProductBrand> productBrandRepo,
        IMapper mapper)
        {
            _ProductRepo = productRepo;
            _ProductTypeRepo = productTypeRepo;
            _ProductBrandRepo = productBrandRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturn>>> GetProducts()
        {
            // var products = await _ProductRepo.ListAllAsync();
            // return Ok(products);
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _ProductRepo.ListAsync(spec);
            return Ok(_mapper
                .Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturn>>(products));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturn>> GetProduct(int id)
        {
            // return await _ProductRepo.GetByIdAsync(id);
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _ProductRepo.GetEntityWithSpecAsync(spec);
            // return new ProductToReturn
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     Price = product.Price,
            //     PictureUrl = product.PictureUrl,
            //     ProductType = product.ProductType.Name,
            //     ProductBrand = product.ProductBrand.Name
            // };
            return _mapper.Map<Product, ProductToReturn>(product);

        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await _ProductBrandRepo.ListAllAsync();
            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await _ProductTypeRepo.ListAllAsync();
            return Ok(types);
        }
    }

}