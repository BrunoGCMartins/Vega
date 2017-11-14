using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using vega.Core;
using vega.Persistence;
using Vega.Controllers.Resources;
using Vega.Core.models;


namespace Vega.Controllers
{
    // /api/vehicles/1/photos
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IHostingEnvironment _host;
        private readonly IVehicleRepository _repository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PhotoSettings photoSettings;

        public PhotosController(IHostingEnvironment host, IVehicleRepository repository, IPhotoRepository photoRepository,
            IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<PhotoSettings> options)
        {
            _host = host;
            _repository = repository;
            _photoRepository = photoRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.photoSettings = options.Value;
            //www.root, where I will put all the uploads
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await _repository.GetVehicle(vehicleId, includeRelated: false);
            if (vehicle == null)
            {
                return NotFound();
            }

            if (file == null)
            {
                return NotFound("Null File");
            }

            if (file.Length == 0)
            {
                return NotFound("Empty File");
            }

            if (file.Length > photoSettings.MaxBytes) //maior q 10 Mb
            {
                return NotFound("Max file size exceeded");
            }

            if (!photoSettings.IsSupported(file.FileName))
            {
                return NotFound("Invalid file type");
            }

            var upladsFolderPath= Path.Combine(_host.WebRootPath, "uploads");

            if (!Directory.Exists(upladsFolderPath)) //Se nao existir essa pasta, simplesmente a criamos
            {
                Directory.CreateDirectory(upladsFolderPath);
            }

            //para proteger do usuario upar qualquer tipo de arquivo
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath= Path.Combine(upladsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo { FileName = fileName };

            vehicle.Photos.Add(photo);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<Photo, PhotoResource>(photo));

        }


        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {
            var photos = await _photoRepository.GetPhotos(vehicleId);

            return _mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }
    }
}
