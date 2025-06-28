using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using VacationsManager.Shared;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;
using YourNamespace.Shared.Repos.Contracts;
using VacationsManager.Shared.Security;

namespace VacationsManagerMVC.Controllers
{
        public abstract class BaseCrudController<TModel, TRepository, TService, TEditVM, TDetailsVM> : Controller
        where TModel : BaseModel
        where TRepository : IBaseRepository<TModel>
        where TService : IBaseCrudService<TModel, TRepository>
        where TEditVM : BaseVM, new()
        where TDetailsVM : BaseVM
        {

            protected readonly TService _service;
            protected readonly IMapper _mapper;

            protected BaseCrudController(TService service, IMapper mapper)
            {
                this._service = service;
                _mapper = mapper;
            }

            protected const int DefaultPageSize = 10;
            protected const int DefaultPageNumber = 1;
            protected const int MaxPageSize = 100;

            public virtual Task<string?> Validate(TEditVM editVM)
            {
                return Task.FromResult<string?>(null);
            }

            protected virtual Task<TEditVM> PrePopulateVMAsync(TEditVM editVM)
            {
                return Task.FromResult(editVM);
            }
            
            [HttpGet]
            public virtual async Task<IActionResult> List(int pageSize = DefaultPageSize, int pageNumber = DefaultPageNumber)
            {
                if (pageSize <= 0 || pageSize > MaxPageSize || pageNumber <= 0)
                {
                    return BadRequest(Constants.InvalidPagination);
                }

                var models = await _service.GetWithPaginationAsync(pageSize, pageNumber);
                var totalRecords = await _service.GetAllAsync();
                var totalPages = (int)Math.Ceiling((double)totalRecords.Count() / pageSize);

                var mappedModels = _mapper.Map<IEnumerable<TDetailsVM>>(models);

                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = pageNumber;

                return View(nameof(List), mappedModels);
            }


            [HttpGet]
            public virtual async Task<IActionResult> Details(int id)
            {
                var model = await this._service.GetByIdIfExistsAsync(id);
                if (model == default)
                {
                    return BadRequest(Constants.InvalidId);
                }
                var mappedModel = _mapper.Map<TDetailsVM>(model);
                return View(mappedModel);

            }

            [HttpGet]
            public virtual async Task<IActionResult> Create()
            {
                var editVM = await PrePopulateVMAsync(new TEditVM());
                return View(editVM);
            }

            [HttpPost]
            public virtual async Task<IActionResult> Create(TEditVM editVM)
            {
                if (!ModelState.IsValid)
                {
                    editVM = await PrePopulateVMAsync(editVM); 
                    return View(editVM);
                }

                var errors = await Validate(editVM);
                if (errors != null)
                {
                    ModelState.AddModelError("", errors);
                    editVM = await PrePopulateVMAsync(editVM);
                    return View(editVM);
                }

                var model = _mapper.Map<TModel>(editVM);
                await _service.SaveAsync(model);
                return await List();
            }
            [HttpGet]
            public virtual async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return BadRequest(Constants.InvalidId);
                }
                var model = await this._service.GetByIdIfExistsAsync(id.Value);
                if (model == default)
                {
                    return BadRequest(Constants.InvalidId);
                }
                var mappedModel = _mapper.Map<TEditVM>(model);
                mappedModel = await PrePopulateVMAsync(mappedModel);
                return View(mappedModel);
            }
            [HttpPost]
            public virtual async Task<IActionResult> Edit(int id, TEditVM editVM)
            {
                if (!ModelState.IsValid)
                {
                    editVM = await PrePopulateVMAsync(editVM); 
                    return View(editVM);
                }

                var errors = await Validate(editVM);
                if (errors != null)
                {
                    ModelState.AddModelError("", errors);
                    editVM = await PrePopulateVMAsync(editVM);
                    return View(editVM);
                }

                if (!await _service.ExistsByIdAsync(id))
                {
                    return BadRequest(Constants.InvalidId);
                }

                var mappedModel = _mapper.Map<TModel>(editVM);
                await _service.SaveAsync(mappedModel);
                return await List();
            }

            [HttpGet]
            public virtual async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return BadRequest(Constants.InvalidId);
                }
                var model = await this._service.GetByIdIfExistsAsync(id.Value);
                if (model == default)
                {
                    return BadRequest(Constants.InvalidId);
                }
                var mappedModel = _mapper.Map<TDetailsVM>(model);
                return View(mappedModel);
            }
            [HttpPost]
            public virtual async Task<IActionResult> Delete(int id)
            {
                if (!await this._service.ExistsByIdAsync(id))
                {
                    return BadRequest(Constants.InvalidId);
                }
                await this._service.DeleteAsync(id);
                return await List();
            }

        }
}
