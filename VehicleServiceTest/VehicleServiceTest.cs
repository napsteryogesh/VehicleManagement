using Moq;
using ScalableVehicleService.DAL;
using ScalableVehicleService.Model;
using System;
using Xunit;
using ScalableVehicleService;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;

namespace VehicleServiceTest
{
    public class VehicleServiceTest
    {

        [Fact]
        public async Task Register_New_Vehicle()
        {
            var dbRepoistory = new Mock<IGenericRepository<Vehicle>>();
            dbRepoistory.Setup(obj => obj.GetAsync(It.IsAny<Expression<Func<Vehicle, bool>>>(),It.IsAny< Func<IQueryable<Vehicle>, IOrderedQueryable<Vehicle>>>(),It.IsAny<string>())).Returns(Task.FromResult(Enumerable.Empty<Vehicle>()));
            dbRepoistory.Setup(obj => obj.InsertAsync(It.IsAny<Vehicle>())).Returns(Task.CompletedTask);

            var vehicleService = new ScalableVehicleService.VehicleService(dbRepoistory.Object);
            bool result = await vehicleService.RegisterAsync(new Vehicle()
            {
                VehicleNumber = "1",
                Location = new Location(100, 200,null)
            });
            Assert.True(result);
        }

        [Fact]
        public async Task Register_Duplicate_Vehicle()
        {
            IEnumerable<Vehicle> vehicles = new List<Vehicle>() { new Vehicle{
                VehicleNumber = "1",
                Location = new Location(100, 200,null)
            }};
            var dbRepoistory = new Mock<IGenericRepository<Vehicle>>();
            dbRepoistory.Setup(obj => obj.GetAsync(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<Func<IQueryable<Vehicle>, IOrderedQueryable<Vehicle>>>(), It.IsAny<string>())).Returns(Task.FromResult(vehicles));
            dbRepoistory.Setup(obj => obj.InsertAsync(It.IsAny<Vehicle>())).Returns(Task.CompletedTask);

            var vehicleService = new ScalableVehicleService.VehicleService(dbRepoistory.Object);
            _ = Assert.ThrowsAsync<Exception>(async () => await vehicleService.RegisterAsync(new Vehicle()
            {
                VehicleNumber = "1",
                Location = new Location(100, 200,null)
            }));
       
        }


        [Fact]
        public async Task Record_New_Location_InvalidVehicle()
        {
            var dbRepoistory = new Mock<IGenericRepository<Vehicle>>();
            dbRepoistory.Setup(obj => obj.GetAsync(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<Func<IQueryable<Vehicle>, IOrderedQueryable<Vehicle>>>(), It.IsAny<string>())).Returns(Task.FromResult(Enumerable.Empty<Vehicle>()));
            dbRepoistory.Setup(obj => obj.UpdateAsync(It.IsAny<Vehicle>())).Returns(Task.CompletedTask);

            var vehicleService = new ScalableVehicleService.VehicleService(dbRepoistory.Object);
            _ = Assert.ThrowsAsync<Exception>(async () => await vehicleService.RecordLocationAsync("1", new Location(400, 600,null)));
        }

        [Fact]
        public async Task Record_New_Location_validVehicle()
        {
            IEnumerable<Vehicle> vehicles = new List<Vehicle>() { new Vehicle{
                VehicleNumber = "1",
                Location = new Location(100, 200,null)
            }};
            var dbRepoistory = new Mock<IGenericRepository<Vehicle>>();
            dbRepoistory.Setup(obj => obj.GetAsync(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<Func<IQueryable<Vehicle>, IOrderedQueryable<Vehicle>>>(), It.IsAny<string>())).Returns(Task.FromResult(vehicles));
            dbRepoistory.Setup(obj => obj.UpdateAsync(It.IsAny<Vehicle>())).Returns(Task.CompletedTask);

            var vehicleService = new ScalableVehicleService.VehicleService(dbRepoistory.Object);
           var result = await vehicleService.RecordLocationAsync("1", new Location(400, 600,null));
            Assert.True(result);
        }



    }
}
