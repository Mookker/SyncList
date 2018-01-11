using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyncList.CommonLibrary.Validation;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Controllers
{
    public class ListItemsApiController : Controller
    {
        private readonly IItemsListRelationsRepository _itemsListRelationsRepository;
        private readonly IListsRepository _listsRepository;
        private readonly IItemsRepository _itemsRepository;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemsListRelationsRepository"></param>
        /// <param name="listsRepository"></param>
        /// <param name="itemsRepository"></param>
        public ListItemsApiController(IItemsListRelationsRepository itemsListRelationsRepository, IListsRepository listsRepository, IItemsRepository itemsRepository)
        {
            _itemsListRelationsRepository = itemsListRelationsRepository;
            _listsRepository = listsRepository;
            _itemsRepository = itemsRepository;
        }
        
                
        /// <summary>
        /// Adds item to list
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/v1/lists/{listId}/items/{itemId}")]
        public async Task<IActionResult> AddItemToList([FromRoute] int listId, [FromRoute] int itemId)
        {
            var listExists = await _listsRepository.Exists(listId);
            var itemExists = await _itemsRepository.Exists(itemId);
            
            Validator.Assert(listExists, ValidationAreas.Exists);
            Validator.Assert(itemExists, ValidationAreas.Exists);

            await _itemsListRelationsRepository.Create(new ItemsListRelation()
            {
                ListId = listId,
                ItemId = itemId,
                IsActive = true
            });
            
            return Ok();
        }
    }
}