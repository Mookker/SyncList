using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyncList.CommonLibrary.Validation;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Controllers
{
    public class ItemsApiController : Controller
    {
        private readonly IItemsRepository _itemsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemsRepository"></param>
        public ItemsApiController(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }
        
        /// <summary>
        /// Gets list of items
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/v1/items")]
        public async Task<IActionResult> GetAllItems([FromQuery]int offset = 0, [FromQuery]int limit = Int32.MaxValue)
        {
            Validator.Assert(offset >= 0 && limit >= 0, ValidationAreas.InputParameters);
                
            var items = await _itemsRepository.GetAll(offset, limit);
            return Ok(items);
        }
        
        /// <summary>
        /// Gets item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/v1/items/{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _itemsRepository.Get(id);

            Validator.Assert(item != null, ValidationAreas.Exists);

            return Ok(item);
        }
        
        /// <summary>
        /// Deletes item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("/v1/items/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _itemsRepository.Get(id);
            Validator.Assert(item != null, ValidationAreas.Exists);
            
            await _itemsRepository.Delete(item);
            
            return Ok();
        }
        
        /// <summary>
        /// Creates item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/v1/items/")]
        public async Task<IActionResult> CreateItem([FromBody]Item item)
        {
            Validator.Assert(item != null, ValidationAreas.InputParameters);
            
            item = await _itemsRepository.Create(item);

            return Ok(item);
        }
        
        /// <summary>
        /// Updates or creates item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/v1/items/{id}")]
        public async Task<IActionResult> UpdateOrCreateItem([FromRoute] int id, [FromBody]Item item)
        {
            Validator.Assert(item != null && item.Id == id && item.Id != 0, ValidationAreas.InputParameters);

            var exists = await _itemsRepository.Exists(id);
            if (exists)
            {
                await _itemsRepository.Update(id, item);
            }
            else
            {
                await _itemsRepository.Create(item);
            }
            
            return Ok(item);
        }
    }
}