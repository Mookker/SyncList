using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SyncList.CommonLibrary.Validation;
using SyncList.SyncListApi.CachingManagement.Interfaces;
using SyncList.SyncListApi.CachingManagement.Models;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Controllers
{
    public class ListItemsApiController : Controller
    {
        private readonly IItemsListRelationsRepository _itemsListRelationsRepository;
        private readonly IListsRepository _listsRepository;
        private readonly IItemsRepository _itemsRepository;
        private readonly IItemsInListCacheManager _itemsInListCacheManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemsListRelationsRepository"></param>
        /// <param name="listsRepository"></param>
        /// <param name="itemsRepository"></param>
        /// <param name="itemsInListCacheManager"></param>
        public ListItemsApiController(IItemsListRelationsRepository itemsListRelationsRepository
            , IListsRepository listsRepository
            , IItemsRepository itemsRepository
            , IItemsInListCacheManager itemsInListCacheManager)
        {
            _itemsListRelationsRepository = itemsListRelationsRepository;
            _listsRepository = listsRepository;
            _itemsRepository = itemsRepository;
            _itemsInListCacheManager = itemsInListCacheManager;
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

        /// <summary>
        /// Gets list + items
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/v1/lists/{listId}/items/")]
        public async Task<IActionResult> GetListWithItems([FromRoute] int listId)
        {
            var existingList = await _listsRepository.Get(listId);
            Validator.Assert(existingList != null, ValidationAreas.Exists);

            var listWithItems = await _itemsInListCacheManager.GetList(listId);
            if (listWithItems == null)
            {
                var result = await _itemsListRelationsRepository.GetListWithItems(listId);
                
                listWithItems = await _itemsInListCacheManager.AddList(existingList);

                if (result != null)
                {
                    foreach (var res in result.ItemList.ItemListRelations)
                    {
                        listWithItems.Items.Add(new CachedItem(res.Item, res.IsActive));
                    }

                    await _itemsInListCacheManager.AddList(listWithItems);
                }

                return Ok(listWithItems);
            }
            return Ok(listWithItems);
        }
        
    }
}