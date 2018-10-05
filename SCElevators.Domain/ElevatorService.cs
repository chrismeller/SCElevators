using System;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using SCElevators.Data;
using SCElevators.Data.Models;

namespace SCElevators.Domain
{
    public class ElevatorService
    {
        private readonly DbConnection _db;

        public ElevatorService(ApplicationDbContext dbContext)
        {
            _db = dbContext.GetDbConnection();
        }

        public async Task<bool> Exists(Int32 number)
        {
            var result =
                await _db.ExecuteScalarAsync<int>("select count(*) from Elevators where Number = @Number",
                    new {Number = number});

            return result == 1;
        }

        public async Task<bool> Update(Int32 number, DateTimeOffset lastSeen)
        {
            var result = await _db.ExecuteAsync("update Elevators set LastSeen = @LastSeen where Number = @Number",
                new {LastSeen = lastSeen, Number = number});

            return result == 1;
        }

        public async Task<bool> Create(Int32 number, string ownerName, string location, string address, string city, string state, string zip, string county, string status, DateTime? nextInspectionDue, string nextInspectionDueOther, string machineType, string manufacturer, string unitType, DateTimeOffset lastSeen)
        {
            var elevator = new Elevator()
            {
                Number = number,
                OwnerName = ownerName,
                Location = location,
                LocationAddress = address,
                LocationCity = city,
                LocationState = state, 
                LocationZip = zip,
                County = county,
                Status = status,
                NextInspectionDue = nextInspectionDue,
                NextInspectionDueOther = nextInspectionDueOther,
                MachineType = machineType,
                Manufacturer = manufacturer,
                UnitType = unitType,

                InsertedAt = DateTimeOffset.UtcNow,
                LastSeen = lastSeen,
            };

            var result = await _db.ExecuteAsync(@"
insert into Elevators (
    Number,
    OwnerName,
    Location,
    LocationAddress,
    LocationCity,
    LocationState,
    LocationZip,
    County,
    Status,
    NextInspectionDue,
    MachineType,
    Manufacturer,
    UnitType,
    InsertedAt,
    LastSeen
) values (
    @Number,
    @OwnerName,
    @Location,
    @LocationAddress,
    @LocationCity,
    @LocationState,
    @LocationZip,
    @County,
    @Status,
    @NextInspectionDue,
    @MachineType,
    @Manufacturer,
    @UnitType,
    @InsertedAt,
    @LastSeen
)", elevator);

            return result == 1;
        }
    }
}