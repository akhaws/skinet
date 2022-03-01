using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.DTOs;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _ProductsRepo;
        private readonly IGenericRepository<ProductType> _ProductType;
        private readonly IGenericRepository<ProductBrand> _ProductBrand;

        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> ProductsRepo, IGenericRepository<ProductBrand> ProductBrandRepo, IGenericRepository<ProductType> ProductTypeRepo, IMapper mapper)
        {
            _ProductsRepo = ProductsRepo;
            _ProductType = ProductTypeRepo;
            _ProductBrand = ProductBrandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams) {

            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countspec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await _ProductsRepo.CountAsync(countspec);
            var products = await _ProductsRepo.ListAsync(spec);
            
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));

            // return products.Select(product => new ProductToReturnDto
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description =  product.Description,
            //     Price = product.Price,
            //     PictureUrl = product.PictureUrl,
            //     ProductType = product.ProductType.Name,
            //     ProductBrand = product.ProductBrand.Name                
            // }).ToList();



        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]  //Usefull For Swagger
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] //Usefull For Swagger
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id) {

            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _ProductsRepo.GetEntityWithSpec(spec);

            if(product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);

            // return new ProductToReturnDto
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description =  product.Description,
            //     Price = product.Price,
            //     PictureUrl = product.PictureUrl,
            //     ProductType = product.ProductType.Name,
            //     ProductBrand = product.ProductBrand.Name
            // };
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProductBrands() {

            var product = await _ProductsRepo.ListAllAsync();

            return Ok(product);
        }        

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProductTypes() {

            var product = await _ProductsRepo.ListAllAsync();

            return Ok(product);
        }        

    }
}