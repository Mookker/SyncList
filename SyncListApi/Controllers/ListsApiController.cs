﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyncList.CommonLibrary.Validation;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ListsApiController : Controller
    {
        private readonly IListsRepository _listsRepository;

        /// <summary>
        /// </summary>
        /// <param name="listsRepository"></param>
        public ListsApiController(IListsRepository listsRepository)
        {
            _listsRepository = listsRepository;
        }
        
        /// <summary>
        /// Gets list of list of lists of lists....
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/v1/lists")]
        public async Task<IActionResult> GetAllLists([FromQuery]int offset = 0, [FromQuery]int limit = Int32.MaxValue)
        {
            Validator.Assert(offset >= 0 && limit >= 0, ValidationAreas.InputParameters);
                
            var lists = await _listsRepository.GetAll(offset, limit);
            return Ok(lists);
        }
        
        /// <summary>
        /// Gets list by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/v1/lists/{id}")]
        public async Task<IActionResult> GetList(int id)
        {
            var list = await _listsRepository.Get(id);
            Validator.Assert(list != null, ValidationAreas.NotExists);
            
            return Ok(list);
        }
        
        /// <summary>
        /// Deletes list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("/v1/lists/{id}")]
        public async Task<IActionResult> DeleteList(int id)
        {
            var list = await _listsRepository.Get(id);
            
            Validator.Assert(list != null, ValidationAreas.NotExists);
            
            await _listsRepository.Delete(list);
            
            return Ok();
        }
        
        /// <summary>
        /// Creates list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/v1/lists/")]
        public async Task<IActionResult> CreateUser([FromBody]ItemList list)
        {
            Validator.Assert(list != null, ValidationAreas.InputParameters);
            
            list = await _listsRepository.Create(list);

            return Ok(list);
        }
        
        /// <summary>
        /// Updates or creates list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/v1/lists/{id}")]
        public async Task<IActionResult> UpdateOrCreateList([FromRoute] int id, [FromBody]ItemList list)
        {
            Validator.Assert(list != null && list.Id == id && list.Id != 0, ValidationAreas.InputParameters);

            var exists = await _listsRepository.Exists(id);
            if (exists)
            {
                await _listsRepository.Update(id, list);
            }
            else
            {
                await _listsRepository.Create(list);
            }
            
            return Ok(list);
        }
    }
}